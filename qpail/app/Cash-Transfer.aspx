<%@ Page Title="" Language="C#" MasterPageFile="~/app/MasterPage.master" AutoEventWireup="true" CodeFile="Cash-Transfer.aspx.cs" Inherits="Cells_Cash_Transfer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


 <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    
            <div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>Cash Transfer to other Branch</h6>
					</div>
					<div class="widget_content">
						<h3>Cash Transfer</h3>
						<%--<p>
							 Cras erat diam, consequat quis tincidunt nec, eleifend a turpis. Aliquam ultrices feugiat metus, ut imperdiet erat mollis at. Curabitur mattis risus sagittis nibh lobortis vel.
						</p>--%>
						<p style="float:right; text-align:right; color:Red;">
						<asp:Label ID="lblMsg" CssClass="msg" runat="server" EnableViewState="false"></asp:Label>
						<asp:Label ID="lblErrLoad" runat="server" CssClass="msg" EnableViewState="false"></asp:Label>
						</p>
						
						<div class="form_container left_label">
							<ul>
							    
								<li>
								<div class="form_grid_12">
									<label class="field_title">Transfer Type:</label>
									<div class="form_input">
                                        
									    <asp:DropDownList ID="ddType" runat="server" data-placeholder="--- Select ---" 
                                             style="width:60%; min-width:100px" Height="25px" class="chzn-select" >
                                            <asp:ListItem>Bank Transfer</asp:ListItem>
                                            <asp:ListItem>Cash Sending</asp:ListItem>
                                        </asp:DropDownList>
										<%--<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [OrderStatement] FROM [Orders] where IsActive='true'"></asp:SqlDataSource>--%>
									</div>
								</div>
								</li>
																
									<li>
								        <div class="form_grid_12">
									        <label class="field_title">Receiver Branch</label>
									        <div class="form_input">
                                                <asp:DropDownList ID="ddBranch" runat="server" data-placeholder="--- Select ---" 
                                             style="width:60%; min-width:150px" Height="25px" class="chzn-select" 
                                                    DataSourceID="SqlDataSource2" DataTextField="BranchName" 
                                                    DataValueField="BranchName" >
                                        </asp:DropDownList>
										<asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:Connection_String %>" 
                                            SelectCommand="SELECT [BranchName] FROM [Branches] where BranchName<>@Branch ORDER BY [BranchName]">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblBranch" Name="Branch" PropertyName="Text" />
                                            </SelectParameters>
                                                </asp:SqlDataSource>
									
                                                
                                                </div>
								        </div>
								    </li>					
								
								<li>
								<div class="form_grid_12">
									<label class="field_title">Description</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtDescription" runat="server" tabindex="2"  title="Transfer Description"  class="limiter tip_top"></asp:TextBox>
										<span class="input_instruction green">Deposit Detail: ie, DBBL-Uttara Branch.</span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Transfer Amount</label>
									<div class="form_input">
                                        <asp:TextBox ID="txtAmount" runat="server" tabindex="3"  title="Transfer Amount"  class="limiter tip_top"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                                runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtAmount">
                                            </asp:FilteredTextBoxExtender>
									</div>
								</div>
								</li>
								
								<li>
								<div class="form_grid_12">
									<div class="form_input">
									
									    <asp:Button ID="btnSave" runat="server" Text="Save Transfer" TabIndex="11"
                                            class="btn_small btn_blue"  onclick="btnSave_Click" />
                                        <asp:Button ID="Button1" runat="server" Text="Clear Form" 
                                            class="btn_small btn_orange" />
									
										<%--<button type="submit" class="btn_small btn_gray"><span>Submit</span></button>
										<button type="reset" class="btn_small btn_gray"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_blue"><span>Sent New Order</span></button>
										<button type="reset" class="btn_small btn_blue"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_orange"><span>Clear Form</span></button>
										<button type="reset" class="btn_small btn_orange"><span>Reset</span></button>--%>
									</div>
								</div>
								</li>
								<asp:Panel ID="Panel2" runat="server" Visible="false">    
								<li>                                                                    
								    <div class="form_grid_12">
									<label class="field_title">Invoice No.</label>
									<div class="form_input">
                                        <asp:Label ID="lblBranch" runat="server" Text=""></asp:Label>
                                        <asp:TextBox ID="InvNo" runat="server" tabindex="10" title="Collected Amount"  
                                            class="limiter tip_top" Font-Bold="True"></asp:TextBox>
										<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                            runat="server" Enabled="True" FilterType="Custom, Numbers" ValidChars="0123456789." TargetControlID="txtCollected">
                                        </asp:FilteredTextBoxExtender>
									</div>
								</div>								
								</li>
								</asp:Panel>
							</ul>
						</div>
					</div>
				</div>
			</div>



<%--
<div class="grid_12 full_block">
				<div class="widget_wrap">
					<div class="widget_top">
						<span class="h_icon list_image"></span>
						<h6>All Form Elements</h6>
					</div>
					<div class="widget_content">
						<h3>Form Header</h3>
						<p>
							 Cras erat diam, consequat quis tincidunt nec, eleifend a turpis. Aliquam ultrices feugiat metus, ut imperdiet erat mollis at. Curabitur mattis risus sagittis nibh lobortis vel.
						</p>
						<div class="form_container left_label">
							<ul>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Text Input</label>
									<div class="form_input">
										<input name="filed01" type="text" tabindex="1" class="limiter"/>
										<span class="input_instruction green">This is an instruction</span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Password Input</label>
									<div class="form_input">
										<input name="filed02" type="password" tabindex="2"/>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Disable Input</label>
									<div class="form_input">
										<input name="filed03" type="text" disabled="disabled"/>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Text input with tooltip</label>
									<div class="form_input">
										<input name="filed04" class="tip_top" title="Input Box" type="text" tabindex="3"/>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Textarea <span class="label_intro">Normal Text Area</span></label>
									<div class="form_input">
										<textarea name="filed05" id="txt_editor" cols="50" rows="5" tabindex="4"></textarea>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Tags </label>
									<div class="form_input">
										<input id="tags_1" type="text" class="tags" value="foo,bar,baz,roffle"/>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Textarea <span class="label_intro">Auto Grow Textarea</span></label>
									<div class="form_input">
										<textarea name="filed06" class="input_grow" cols="50" rows="5" tabindex="5"></textarea>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Text Input <span class="label_intro">Text Input With Placeholder</span></label>
									<div class="form_input">
										<input name="filed07" placeholder="Placholder Text" type="text" tabindex="6"/>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Checkbox</label>
									<div class="form_input">
										<span>
										<input name="field08" class="checkbox" type="checkbox" value="First" tabindex="7">
										<label class="choice">First</label>
										</span><span>
										<input name="field09" class="checkbox" type="checkbox" value="Second" tabindex="8">
										<label class="choice">Second</label>
										</span><span>
										<input name="field10" class="checkbox" type="checkbox" value="Third" tabindex="9">
										<label class="choice">Third</label>
										</span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Radio Button</label>
									<div class="form_input">
										<span>
										<input name="field11" class="radio" type="radio" value="First" tabindex="10">
										<label class="choice">First</label>
										</span><span>
										<input name="field12" class="radio" type="radio" value="Second" tabindex="11">
										<label class="choice">Second</label>
										</span><span>
										<input name="field13" class="radio" type="radio" value="Third" tabindex="12">
										<label class="choice">Third</label>
										</span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Select Box <span class="label_intro">This is a seletbox</span></label>
									<div class="form_input">
										<select data-placeholder="Your Favorite Football Team" style=" width:300px" class="chzn-select" tabindex="13">
											<option value=""></option>
											<optgroup label="NFC EAST">
											<option>Dallas Cowboys</option>
											<option>New York Giants</option>
											<option>Philadelphia Eagles</option>
											<option>Washington Redskins</option>
											</optgroup>
											<optgroup label="NFC NORTH">
											<option>Chicago Bears</option>
											<option>Detroit Lions</option>
											<option>Green Bay Packers</option>
											<option>Minnesota Vikings</option>
											</optgroup>
											<optgroup label="NFC SOUTH">
											<option>Atlanta Falcons</option>
											<option>Carolina Panthers</option>
											<option>New Orleans Saints</option>
											<option>Tampa Bay Buccaneers</option>
											</optgroup>
											<optgroup label="NFC WEST">
											<option>Arizona Cardinals</option>
											<option>St. Louis Rams</option>
											<option>San Francisco 49ers</option>
											<option>Seattle Seahawks</option>
											</optgroup>
											<optgroup label="AFC EAST">
											<option>Buffalo Bills</option>
											<option>Miami Dolphins</option>
											<option>New England Patriots</option>
											<option>New York Jets</option>
											</optgroup>
											<optgroup label="AFC NORTH">
											<option>Baltimore Ravens</option>
											<option>Cincinnati Bengals</option>
											<option>Cleveland Browns</option>
											<option>Pittsburgh Steelers</option>
											</optgroup>
											<optgroup label="AFC SOUTH">
											<option>Houston Texans</option>
											<option>Indianapolis Colts</option>
											<option>Jacksonville Jaguars</option>
											<option>Tennessee Titans</option>
											</optgroup>
											<optgroup label="AFC WEST">
											<option>Denver Broncos</option>
											<option>Kansas City Chiefs</option>
											<option>Oakland Raiders</option>
											<option>San Diego Chargers</option>
											</optgroup>
										</select>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Multiple Select</label>
									<div class="form_input">
										<select data-placeholder="Your Favorite Types of Bear" style="width:300px;" multiple class="chzn-select" tabindex="14">
											<option value=""></option>
											<option>American Black Bear</option>
											<option>Asiatic Black Bear</option>
											<option>Brown Bear</option>
											<option>Giant Panda</option>
											<option selected>Sloth Bear</option>
											<option disabled>Sun Bear</option>
											<option selected>Polar Bear</option>
											<option disabled>Spectacled Bear</option>
										</select>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Multiple Select with Groups</label>
									<div class="form_input">
										<select data-placeholder="Your Favorite Football Team" style="width:300px;" class="chzn-select" multiple tabindex="15">
											<option value=""></option>
											<optgroup label="NFC EAST">
											<option>Dallas Cowboys</option>
											<option>New York Giants</option>
											<option>Philadelphia Eagles</option>
											<option>Washington Redskins</option>
											</optgroup>
											<optgroup label="NFC NORTH">
											<option>Chicago Bears</option>
											<option>Detroit Lions</option>
											<option>Green Bay Packers</option>
											<option>Minnesota Vikings</option>
											</optgroup>
											<optgroup label="NFC SOUTH">
											<option>Atlanta Falcons</option>
											<option>Carolina Panthers</option>
											<option>New Orleans Saints</option>
											<option>Tampa Bay Buccaneers</option>
											</optgroup>
											<optgroup label="NFC WEST">
											<option>Arizona Cardinals</option>
											<option>St. Louis Rams</option>
											<option>San Francisco 49ers</option>
											<option>Seattle Seahawks</option>
											</optgroup>
											<optgroup label="AFC EAST">
											<option>Buffalo Bills</option>
											<option>Miami Dolphins</option>
											<option>New England Patriots</option>
											<option>New York Jets</option>
											</optgroup>
											<optgroup label="AFC NORTH">
											<option>Baltimore Ravens</option>
											<option>Cincinnati Bengals</option>
											<option>Cleveland Browns</option>
											<option>Pittsburgh Steelers</option>
											</optgroup>
											<optgroup label="AFC SOUTH">
											<option>Houston Texans</option>
											<option>Indianapolis Colts</option>
											<option>Jacksonville Jaguars</option>
											<option>Tennessee Titans</option>
											</optgroup>
											<optgroup label="AFC WEST">
											<option>Denver Broncos</option>
											<option>Kansas City Chiefs</option>
											<option>Oakland Raiders</option>
											<option>San Diego Chargers</option>
											</optgroup>
										</select>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Deselect on Single Selects</label>
									<div class="form_input">
										<select data-placeholder="Your Favorite Type of Bear" style="width:300px;" class="chzn-select-deselect" tabindex="16">
											<option value=""></option>
											<option>American Black Bear</option>
											<option>Asiatic Black Bear</option>
											<option>Brown Bear</option>
											<option>Giant Panda</option>
											<option selected>Sloth Bear</option>
											<option>Sun Bear</option>
											<option>Polar Bear</option>
											<option>Spectacled Bear</option>
										</select>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Group Radio</label>
									<div class="form_input">
										<div class="form_grid_2 alpha">
											<fieldset>
												<legend>Legend</legend>
												<span class="column_input">
												<input name="field14" class="radio" type="radio" value="First" tabindex="17">
												<label class="choice">First</label>
												</span><span class="column_input">
												<input name="field14" class="radio" type="radio" value="Second" tabindex="18">
												<label class="choice">Second</label>
												</span><span class="column_input">
												<input name="field14" class="radio" type="radio" value="Third" tabindex="19">
												<label class="choice">Third</label>
												</span>
											</fieldset>
										</div>
										<div class="form_grid_2">
											<fieldset>
												<legend>Legend</legend>
												<span class="column_input">
												<input name="field15" class="radio" type="radio" value="First" tabindex="20">
												<label class="choice">First</label>
												</span><span class="column_input">
												<input name="field15" class="radio" type="radio" value="Second" tabindex="21">
												<label class="choice">Second</label>
												</span><span class="column_input">
												<input name="field15" class="radio" type="radio" value="Third" tabindex="22">
												<label class="choice">Third</label>
												</span>
											</fieldset>
										</div>
										<div class="form_grid_2">
											<fieldset>
												<legend>Legend</legend>
												<span class="column_input">
												<input name="field16" class="radio" type="radio" value="First" tabindex="23">
												<label class="choice">First</label>
												</span><span class="column_input">
												<input name="field16" class="radio" type="radio" value="Second" tabindex="24">
												<label class="choice">Second</label>
												</span><span class="column_input">
												<input name="field16" class="radio" type="radio" value="Third" tabindex="25">
												<label class="choice">Third</label>
												</span>
											</fieldset>
										</div>
										<span class="clear"></span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">File Input</label>
									<div class="form_input">
										<input name="file01" type="file">
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Rating</label>
									<div class="form_input">
										<div id="star">
										</div>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Dual List</label>
									<div class="form_input">
										<div class="list_left">
											<div class="list_filter">
												<label>Filter: </label>
												<input type="text" id="box1Filter"/>
												<span id="box1Clear" class="list_refresh"><span class="filter_btn refresh_3">&nbsp;</span></span>
											</div>
											<select id="box1View" multiple="multiple" class="multiple_list">
												<option value="501649">2008-2009 "Mini" Baja</option>
												<option value="501497">AAPA - Asian American Psychological Association</option>
												<option value="501053">Academy of Film Geeks</option>
												<option value="500001">Accounting Association</option>
												<option value="501227">ACLU</option>
												<option value="501610">Active Minds</option>
												<option value="501514">Activism with A Reel Edge (A.W.A.R.E.)</option>
												<option value="501656">Adopt a Grandparent Program</option>
												<option value="501050">Africa Awareness Student Organization</option>
												<option value="501075">African Diasporic Cultural RC Interns</option>
												<option value="501493">Agape</option>
												<option value="501562">AGE-Alliance for Graduate Excellence</option>
												<option value="500676">AICHE (American Inst of Chemical Engineers)</option>
												<option value="501460">AIDS Sensitivity Awareness Project ASAP</option>
												<option value="500004">Aikido Club</option>
												<option value="500336">Akanke</option>
											</select>
											<div class="list_itemcount">
												<span id="box1Counter" class="countLabel"></span>
												<select id="box1Storage">
												</select>
											</div>
										</div>
										<div class="list_switch">
											<span id="to2" class="swap_btn"><span class="filter_btn p_next">&nbsp;</span></span><span id="to1" class="swap_btn"><span class="filter_btn p_prev">&nbsp;</span></span><span id="allTo2" class="swap_btn"><span class="filter_btn p_last">&nbsp;</span></span><span id="allTo1" class="swap_btn"><span class="filter_btn p_first">&nbsp;</span></span>
										</div>
										<div class="list_right">
											<div class="list_filter">
												<label>Filter: </label>
												<input type="text" id="box2Filter"/>
												<span class="list_refresh" id="box2Clear"><span class="filter_btn refresh_3">&nbsp;</span></span>
											</div>
											<select id="box2View" multiple="multiple" class="multiple_list">
											</select>
											<div class="list_itemcount">
												<span id="box2Counter" class="countLabel"></span>
												<select id="box2Storage">
												</select>
											</div>
										</div>
										<span class="clear"></span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Spinner</label>
									<div class="form_input">
										<input type="text" class="spinner" id="spinner" value="0"/>
										<input type="text" id="spinnerhide" value="0" class="spinner"/>
										<input type="text" class="spinner" id="spinnercurrency"/>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Date Picker</label>
									<div class="form_input">
										<div class=" form_grid_2 alpha">
											<input name="filed30" type="text" class="datepicker"/>
											<span class=" label_intro">From</span>
										</div>
										<div class=" form_grid_2">
											<input name="filed31" type="text" class="datepicker"/>
											<span class=" label_intro">To</span>
										</div>
										<span class="clear"></span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Iphone Style Button</label>
									<div class="form_input on_off">
										<input type="checkbox" checked="checked" id="on_off_on"/>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">iButton</label>
									<div class="form_input field switch">
										<label class="cb-enable selected"><span>Enable</span></label>
										<label class="cb-disable"><span>Disable</span></label>
										<span class="clear"></span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<label class="field_title">Mask Input</label>
									<div class="form_input">
										<div class="form_grid_2 alpha">
											<input name="date" type="text" id="date"/>
											<span class=" label_intro">Date</span>
										</div>
										<div class="form_grid_3">
											<input name="phone" type="text" id="phone"/>
											<span class=" label_intro">Phone</span>
										</div>
										<div class="form_grid_2">
											<input name="tin" type="text" id="tin"/>
											<span class=" label_intro">Tin</span>
										</div>
										<div class="form_grid_2">
											<input name="ssn" type="text" id="ssn"/>
											<span class=" label_intro">SSN</span>
										</div>
										<span class="clear"></span>
									</div>
								</div>
								</li>
								<li>
								<div class="form_grid_12">
									<div class="form_input">
										<button type="submit" class="btn_small btn_gray"><span>Submit</span></button>
										<button type="reset" class="btn_small btn_gray"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_blue"><span>Submit</span></button>
										<button type="reset" class="btn_small btn_blue"><span>Reset</span></button>
										<button type="submit" class="btn_small btn_orange"><span>Submit</span></button>
										<button type="reset" class="btn_small btn_orange"><span>Reset</span></button>
									</div>
								</div>
								</li>
							</ul>
						</div>
					</div>
				</div>
			</div>
--%>

</asp:Content>

