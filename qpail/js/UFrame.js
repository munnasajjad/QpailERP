﻿/*
UFrame 1.1.0
 
Copyright (c) 2008 Omar AL Zabir (omaralzabir.com)
GPL License
 
Usage:

UFrame.init({
id: "iPanel1",  // id of the DIV
		
loadFrom: "somePage.aspx",  // Initial page to load from
initialLoad : "GET",    // Initial load mode
showProgress : true,    // Whether to show the progressTemplate
		
beforeLoad: function(url,data) { callback() },  // callback fired before content is loaded from loadFrom
afterLoad: function(data, response) { callback() }, // callback fired after response is loaded from loadFrom
beforePost: function(url,data) { callback() },  // callback fired before content is posted
afterPost: function(data, response) { callback() }, // callback fired after content is posted and response is available
		
params : { "Gaga" : "gugu" },   // parameters to post/get to 
		
progressTemplate : "<p>Loading...<p>",  // template shown when content is being loaded from or posted to
		
beforeBodyTemplate : "<p>This is rendered before the body</p>", // added before any response body
afterBodyTemplate : "<p>This is rendered after the body</p>",   // added after any response body
		
});
*/

(function () {

    UFrame = function (config) {
        this.config = config;
    };

    UFrame.prototype = {
        load: function () {
            var c = this.config;
            if (c.loadFrom) {
                UFrameManager.loadHtml(c.loadFrom, c.params, c);
            }
        },
        submit: function (form) {
            UFrameManager.submitForm(form, null);
        },
        navigate: function (href) {
            UFrameManager.loadHtml(href, null, this.config);
        }
    }

    UFrameManager =
{
    _uFrames: {},

    empty: function () { },

    init: function (config) {
        var o = new UFrame(config);
        UFrameManager._uFrames[config.id] = o;
        o.load();
    },
    getHtml: function (url, queryString, callback) {
        try {
            $.ajax({
                url: url,
                type: "GET",
                data: queryString,
                dataType: "html",
                success: callback,
                error: function (xml, status, e) {
                    //alert("Error occured while loading: " + url +'\n' + xml.status + ":" + xml.statusText + '\n' + xml.responseText); 
                    if (xml && xml.responseText)
                        callback(xml.responseText);
                    else
                        alert("Error occured while loading: " + url + '\n' + e.message);
                },
                cache: true
            });
        } catch (e) {
            alert(e);
        }
    },

    getUFrame: function (id) {
        return UFrameManager._uFrames[id];
    },

    submitForm: function (form, submitData) {
        // Find all checked checkbox, radio button, text box, hidden fild, password box and submit button
        // collect all their names and values 
        var params = {};
        $(form)
        //.find("input[@checked], input[@type='text'], input[@type='hidden'], input[@type='password'], input[@type='submit'], option[@selected], textarea")
	        .find("input[checked], input[type='text'], input[type='hidden'], input[type='password'], option[selected], textarea")
	        .filter(":enabled")
	        .each(function () {
	            params[this.name || this.id || this.parentNode.name || this.parentNode.id] = this.value;
	        });

        if (submitData)
            params[submitData.name] = submitData.value;

        var uframeId = $(form).attr("UFrameID");
        var uframe = UFrameManager.getUFrame(uframeId);

        var config = uframe.config;
        var container = $('#' + config.id);

        var url = form.action;
        if ((config.beforeLoad || UFrameManager.empty)(url, params) !== false) {
            if (config.progressTemplate) container.html(config.progressTemplate);

            $.post(url, params, function (data) {
                (config.afterLoad || UFrameManager.empty)(url, data);
                UFrameManager.processHtml(data, container, config);
            });
        }
    },

    loadHtml: function (url, params, config) {
        var container = $('#' + config.id);
        var queryString = $.param(params || {});

        if ((config.beforeLoad || UFrameManager.empty)(url, params) !== false) {
            //if(config.progressTemplate) container.html(config.progressTemplate);

            UFrameManager.getHtml(url, queryString, function (content) {
                (config.afterLoad || UFrameManager.empty)(url, content);
                UFrameManager.processHtml(content, container, config);
            });
        }
    },

    processHtml: function (content, container, config) {
        var result = UFrameManager.parseHtml(content, config);

        var head = document.getElementsByTagName('head')[0];

        $(result.styles).each(function (index, text) {
            var styleNode = document.createElement("style");
            styleNode.setAttribute("type", "text/css");
            if (styleNode.styleSheet) // IE
            {
                styleNode.styleSheet.cssText = text;
            }
            else // w3c
            {
                var cssText = document.createTextNode(text);
                styleNode.appendChild(cssText);
            }

            head.appendChild(styleNode);
        });

        $(result.links).each(function (index, attrs) {
            window.setTimeout(function () {
                var link = document.createElement('link');
                for (var i = 0; i < attrs.length; i++) {
                    var attr = attrs[i];
                    link.setAttribute("" + attr.name, "" + attr.value);
                }

                if (link.href)
                    if (!UFrameManager.isTagLoaded('link', 'href', link.href))
                        head.appendChild(link);
            }, 0);
        });

        var scriptsToLoad = result.externalScripts.length;

        $(result.externalScripts).each(function (index, scriptSrc) {
            if (UFrameManager.isTagLoaded('script', 'src', scriptSrc)) {
                scriptsToLoad--;
            }
            else {
                $.ajax({
                    url: scriptSrc,
                    type: "GET",
                    data: null,
                    dataType: "script",
                    success: function () { scriptsToLoad--; },
                    error: function () { scriptsToLoad--; },
                    cache: true
                });
            }
        });

        // wait until all the external scripts are downloaded
        UFrameManager.until({
            test: function () { return scriptsToLoad === 0; },
            delay: 100,
            callback: function () {
                // render the body
                var html = (config.beforeBodyTemplate || "") + result.body + (config.afterBodyTemplate || "");
                container.html(html);

                window.setTimeout(function () {
                    // execute all inline scripts 
                    $(result.inlineScripts).each(function (index, script) {
                        $.globalEval(script);
                    });

                    UFrameManager.hook(container, config);

                    if (typeof callback == "function") callback();
                }, 0);
            }
        });
    },

    isTagLoaded: function (tagName, attName, value) {
        // Create a temporary tag to see what value browser eventually 
        // gives to the attribute after doing necessary encoding
        var tag = document.createElement(tagName);
        tag[attName] = value;
        var tagFound = false;
        $(tagName, document).each(function (index, t) {
            if (tag[attName] === t[attName]) { tagFound = true; return false }
        });
        return tagFound;
    },

    hook: function (container, config) {
        // Add an onclick event on all <a> 
        $("a", container)
        .unbind("click")
        .click(function () {
            var href = $(this).attr("href");
            if (href) {
                if (href.indexOf('javascript:') !== 0) {
                    UFrameManager.loadHtml(href, null, config);
                    return false;
                }
                else if (UFrameManager.executeASPNETPostback(this, href)) {
                    return false;
                }
                else
                    return true;
            }
            else {
                return true;
            }
        });

        // Hook all button type things that can post the form
        $(":image,:submit,:button", container)
            .unbind("click")
            .click(function () {
                return UFrameManager.submitInput(this);
            });


        // Only for IE6 : enter key invokes submit event
        $("form", container)
            .attr("UFrameID", config.id)
            .unbind("submit")
            .submit(function () {
                var firstInput = $(":image,:submit,:button", container).get(0);
                return UFrameManager.submitInput(firstInput);
            });

    },

    executeASPNETPostback: function (input, href) {
        if (href.indexOf("__doPostBack") > 0) {
            // ASP.NET Postback. Collect the values being posted and submit them manually
            var parts = href.split("'");
            var eventTarget = parts[1];
            var eventArgument = parts[3];

            var form = $(input).parents("form").get(0);
            form.__EVENTTARGET.value = unescape(eventTarget);
            form.__EVENTARGUMENT.value = unescape(eventArgument);
            UFrameManager.submitForm(form, null);
            return true;
        }
        else {
            return false;
        }
    },

    submitInput: function (input) {
        var form = input.form;
        if (form.onsubmit && form.onsubmit() == false) {
            return false;
        }

        input = $(input);
        UFrameManager.submitForm(form, { name: input.attr("name"), value: input.attr("value") });

        return false;
    },

    until: function (o /* o = { test: function(){...}, delay:100, callback: function(){...} } */) {
        if (o.test() === true) o.callback();
        else window.setTimeout(function () { UFrameManager.until(o); }, o.delay || 100);
    },

    delay: function (func, delay) {
        window.setTimeout(func, delay || 100);
    },

    parseHtml: function (content) {
        var result = { body: "", externalScripts: [], inlineScripts: [], links: [], styles: [] };

        var bodyContent = [];
        var bodyStarted = false;

        var inlineScriptStarted = false;
        var inlineScriptContent = [];

        var inlineStyleStarted = false;
        var inlineStyleContent = [];

        HTMLParser(content, {
            start: function (tag, attrs, unary) {
                if (tag == "body") {
                    bodyStarted = true;
                }
                else if (tag == "script") {
                    var srcFound = false;
                    $(attrs).each(function (index, attr) {
                        if (attr.name == "src") {
                            result.externalScripts.push(attr.value);
                            srcFound = true;
                        }
                    });
                    if (!srcFound) {
                        // inline script
                        inlineScriptStarted = true;
                        inlineScriptContent = [];
                    }
                }
                else if (tag == "link") {
                    result.links.push(attrs);
                }
                else if (tag == "style") {
                    // inline style node
                    inlineStyleStarted = true;
                    inlineStyleContent = [];
                }
                else {
                    if (bodyStarted) {
                        var attributes = [];
                        for (var i = 0; i < attrs.length; i++)
                            attributes.push(attrs[i].name + '="' + attrs[i].value + '"');

                        bodyContent.push("<" + tag + " " + attributes.join(" ") + (unary ? "/" : "") + ">");
                    }
                }
            },
            end: function (tag) {
                if (tag == "script") {
                    if (inlineScriptStarted) {
                        inlineScriptStarted = false;
                        result.inlineScripts.push(inlineScriptContent.join("\n"));
                    }
                }
                else if (tag == "style") {
                    inlineStyleStarted = false;
                    result.styles.push(inlineStyleContent.join("\n"));
                }
                else {
                    if (bodyStarted)
                        bodyContent.push("</" + tag + ">");
                }
            },
            chars: function (text) {
                if (inlineScriptStarted)
                    inlineScriptContent.push(text);
                else if (inlineStyleStarted)
                    inlineStyleContent.push(text);
                else if (bodyStarted)
                    bodyContent.push(text);
            },
            comment: function (text) {
            }
        });

        result.body = bodyContent.join("\n");
        return result;
    },
    initContainers: function () {
        $('div[src]', document).each(function () {
            var container = $(this);
            var id = container.attr("id");
            if (null == UFrameManager._uFrames[id]) {
                UFrameManager.init({
                    id: id,

                    loadFrom: container.attr("src"),
                    initialLoad: "GET",

                    progressTemplate: container.attr("progressTemplate") || null,

                    showProgress: container.attr("showProgress") || false,

                    beforeLoad: function (url, data) { return eval(container.attr("beforeLoad") || "true") },
                    afterLoad: function (data, response) { return eval(container.attr("afterLoad") || "true") },
                    beforePost: function (url, data) { return eval(container.attr("beforePost") || "true") },
                    afterPost: function (data, response) { return eval(container.attr("afterPost") || "true") },

                    params: null,

                    beforeBodyTemplate: container.attr("beforeBodyTemplate") || null,
                    afterBodyTemplate: container.attr("afterBodyTemplate") || null
                });
            }
        });
    }
};

    $(function () {
        UFrameManager.initContainers();
    });

})();