<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="reset_pass.aspx.cs" Inherits="reset_pass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" Runat="Server">

    
    <div id="pannel-left1">
      <div id="thankyou"><br />
          <br />
          <br />
          <div class="top-img"><img src="images/forgot-password.gif" alt="Forgot Password" width="681" height="78" /></div>
        <div class="top-middle">
          <div class="thank-text-2">
              <div class="thank-form3">
                <div class="thank-form-top">
                  <div class="thank-form-top1"><img src="images/thank-top.jpg" alt="" /></div>
                  <div class="thank-form-top2">
                     
                       <table width="100%" border="0" cellspacing="0" cellpadding="0" class="login">
                         <tr>
                           <td><!--<table width="100%" border="0" cellpadding="0" cellspacing="0" > -->
                               <!--<tr>
                                 <td class="org-bdr" style="padding-left:0px;" align="center"><strong>&nbsp;&nbsp;&nbsp;&nbsp;Password Retrieval </strong> -->
                                   <!-- <a href="javascript:void()" onClick="parent.parent.GB_hide();" ><img src="greybox/close.gif" alt="" width="23" height="24" border="0" /></a> -->
                                 <!--&nbsp;</td>
                               </tr> -->
                         <!--  </table> --></td>
                         </tr>


                           <asp:Panel ID="Panel1" runat="server">

                                                  <tr>
                           <td colspan='2' align='left' valign="top" class='red' style='padding-left:95px; padding-top:20px;'><div class='error'>
                               <asp:Literal ID="Literal1" runat="server"></asp:Literal></div>                               <div class='error'>&nbsp;</div>                           </td>
                         </tr>
                         <%--<tr style="display:;">
                           <td width="35%" height="31" align="left" valign="top" class="arialnew" style="padding-left:25px;">Username :</td>
                           <td width="73%" align="left" valign="top"><div align="left" style="padding-left:10px;">
                               <asp:TextBox ID="txtUserName" runat="server" CssClass="input"></asp:TextBox>
                               
                             </div>
                               <div align="left"></div></td>
                         </tr>--%>

                                            <tr>
                                                <td align="right" class="arialnew" height="30">New Login Password:</td>
                                                <td align="left">                                                    
                                                    <asp:TextBox ID="txtPass" TextMode="Password" runat="server" title="TxtLenMinAndMaxM::Please Enter Password::5::15 :: Characters for Password." maxlength="15" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="arialnew" height="30">Verify New password:</td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPassConfirm" TextMode="Password" runat="server" title="TxtLenMinAndMaxM::Please Confirm Password::5::15 :: Characters for Password." maxlength="15" />    
                                                </td>
                                            </tr>


                         <tr style="display:;">
                           <td height="55"   >&nbsp;</td>
                           <td align="left" valign="top"  style="padding-left:10px;" >                               
                               <asp:ImageButton ID="btnSubmit" runat="server" CssClass="btn-sbmt" ImageUrl="images//btn-sbmt.gif" OnClick="btnSubmit_Click" />                               
                           </td>
                         </tr>
</asp:Panel>


                           <asp:Panel ID="Panel2" runat="server" Visible="false">

                               <tr>
                           <td colspan="2" height="35" align="center" style="color:red">A password reset link has been sent to
                                <asp:Literal ID="Literal2" runat="server" Visible="false"/><br><br>
		                        Link Expired!
		</td>
                         </tr>

                               </asp:Panel>


            <!--<tr>
              <td height="15" colspan="2" align="center" ><br />
              <span class="black"><strong>Member Login</strong></span> <a href="member_login.aspx" class="green">click here</a></td>
            </tr>-->
                       </table>
                     
                  </div>
                  <div class="thank-form-top1"><img src="images/thank-bootom.jpg" alt="" /></div>
                </div>
              </div>
            <div class="key"><img src="images/key.jpg" alt="" /></div>
          </div>
        </div>
        <div class="bottom-img"><img src="images/thankyou-bootom.jpg" alt="" /></div>
      </div>
    </div>
  </div>




</asp:Content>

