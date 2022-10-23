using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;
using RunQuery;
using XERPSecurity;

public partial class AdminCentral_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                generatePageElements();
                Branding_Settings();
                Session["ProjectID"] = SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + Page.User.Identity.Name.ToString() + "'");

                string sessionID = Session["ProjectID"] as string;
                string lName = Page.User.Identity.Name.ToString();

                if (sessionID == null)
                {
                    Session.Abandon();
                    Session.Contents.RemoveAll();
                    System.Web.Security.FormsAuthentication.SignOut();
                    Response.Redirect("../Login.aspx");
                }
                else
                {
                    LoadMenu();
                    LoadPermission();
                    string branch = SQLQuery.ReturnString("Select ProjectName from Projects where VID=(Select ProjectID from Logins where LoginUserName='" + lName + "')");
                    string title = Page.Title;
                    if (string.IsNullOrEmpty(title))
                    {
                        Page.Title = lName + " : " + branch;
                    }
                }

                lblUser.Text = lName;

                //Restrict User
                //string permLevel = SQLQuery.ReturnString("Select UserLevel from Logins where LoginUserName='" + lName + "'");
                //if (Convert.ToInt32(permLevel)>1)//Permitted Only for super Admin
                //{
                //    IncomeStatement.Attributes.Add("class","hidden");
                //    TrialBalance.Attributes.Add("class", "hidden");
                //    BalanceSheet.Attributes.Add("class", "hidden");
                //    Li56.Attributes.Add("class", "hidden");
                //    ALLEntries.Attributes.Add("class", "hidden");
                //}


            }

        }
        catch (Exception ex)
        {
            Notify(ex.Message.ToString(), "error");
        }
    }

    //Message & Notify For Alerts
    private void Notify(string msg, string type)
    {
        ScriptManager.RegisterClientScriptBlock(this, GetType(), "Sc", "$.notify('" + msg + "','" + type + "');", true);
    }

    protected void LinkButtonx_Click(object sender, EventArgs e)
    {
        // Set current datetime to last login datetime
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmdxx = new SqlCommand("Select CurrentLoginTime from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        DateTime lDate = Convert.ToDateTime(cmdxx.ExecuteScalar());
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();

        DateTime outTime = DateTime.Now;
        TimeSpan timeDiff = outTime.Subtract(lDate);
        string stayTime = String.Format("{0}:{1}:{2}", timeDiff.Hours, timeDiff.Minutes, timeDiff.Seconds);

        //Updating Login Datetime
        SqlCommand cmd = new SqlCommand("update LoginHistory set OutTime=@OutTime, WorkingTimeHr=@wh WHERE LID=(Select IsNull(Max(LID),0) from LoginHistory WHERE MemberID =@LName)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmd.Parameters.Add("@OutTime", SqlDbType.DateTime).Value = outTime;
        cmd.Parameters.Add("@wh", SqlDbType.VarChar).Value = stayTime;
        cmd.Parameters.Add("@LName", SqlDbType.VarChar).Value = lName;
        cmd.Connection.Open();
        cmd.ExecuteNonQuery();
        cmd.Connection.Close();
        cmd.Connection.Dispose();

        Session.Abandon();
        Session.Contents.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("../Login.aspx");
    }

    private void generatePageElements()
    {
        //Get Branch/Centre Name
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmdxx = new SqlCommand("Select ProjectName from Projects where VID=(Select ProjectID from Logins where LoginUserName='" + lName + "')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        string branch = Convert.ToString(cmdxx.ExecuteScalar());
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();

        lblPOrder.Text = SQLQuery.ReturnString("Select ISnull(count(SaleID),0) from Sales where InvDate='" + DateTime.Now.ToString("yyyy-MM-dd") + "'");

        SqlCommand cmdxxz1 = new SqlCommand("Select ISnull(count(MsgID),0) from Messaging where Receiver='" + lName + "' AND IsRead='0'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxxz1.Connection.Open();
        lblPendingMsg.Text = Convert.ToString(cmdxxz1.ExecuteScalar());
        cmdxxz1.Connection.Close();
        cmdxxz1.Connection.Dispose();

        // Set current datetime to last login datetime
        SqlCommand cmdx = new SqlCommand("Select LastLoginDate from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdx.Connection.Open();
        string lDate = Convert.ToString(cmdx.ExecuteScalar());
        cmdx.Connection.Close();
        cmdx.Connection.Dispose();

        if (lDate == "")
        {
            lblLogedIn.Text = "Never Logged-in";
        }
        else
        {
            lblLogedIn.Text = lDate;
        }

    }



    private void Branding_Settings()
    {
        try
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP (1) sid, DevelopedBy, ProviderAddress, LoginLogo, InnerLogo, SoftwareName, SoftwareMode, ProviderURL, TrialDate FROM settings_branding where IsActive=1", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd.Connection.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                //string sid = dr[0].ToString();
                string provider = dr[1].ToString();
                //string addr = dr[2].ToString();
                //string logo = dr[3].ToString();
                string logo = dr[4].ToString();
                string sName = dr[5].ToString();

                string sMode = dr[6].ToString();
                string url = dr[7].ToString();
                string tDate = dr[8].ToString();

                ltrDeveloper.Text = "&copy;" + DateTime.Now.Year.ToString() + " <a href='" + url + "' target='_blank'>" + provider + "</a>";
                imgLogo.ImageUrl = "../branding/" + logo;
                imgLogo.AlternateText = sName + " by " + provider;

            }

            cmd.Connection.Close();
            cmd.Connection.Dispose();
        }
        catch (Exception ex)
        { }
    }

    private void LoadPermission()
    {

        //XERP User Security Model
        //------------------------
        //1. Super Admin: All access
        //2. Admin: CRUD, Only cant create admin account, Menu access assignment
        //3. Super User: CRUD except delete, no admin feature access
        //4. User: Can Write & read, no update/delete
        //5. Guest: only reports for specific department features
        try
        {
            string lName = Page.User.Identity.Name.ToString();
            int permissionLevel = XERPSecure.CheckPermissionLevel(lName);
            if (permissionLevel > 2) // Normal User
            {
                Admin.Attributes.Add("class", "hidden");

                //Meny Access Permission For Lower then admin accounts
                //---------------
                //2	Sales
                //3	Purchase
                //4	Store & Inventory
                //5	Production
                //6	Accounts
                //7	HRM & Payroll

                //XERPSecure.HideMainMenu(lName, "2", SalesMenuPanel);
                //XERPSecure.HideMainMenu(lName, "3", PurchaseMenuPanel);
                //XERPSecure.HideMainMenu(lName, "4", InventoryMenuPanel);
                //XERPSecure.HideMainMenu(lName, "5", ProductionMenuPanel);
                //XERPSecure.HideMainMenu(lName, "6", AccountsMenuPanel);
                if (XERPSecure.HideMainMenu(lName, "2") == 1)
                {
                    salesMenu.Attributes.Add("class", "hidden");
                }

                if (XERPSecure.HideMainMenu(lName, "3") == 1)
                {
                    purchaseMenu.Attributes.Add("class", "hidden");
                }

                if (XERPSecure.HideMainMenu(lName, "4") == 1)
                {
                    Inventory.Attributes.Add("class", "hidden");
                }

                if (XERPSecure.HideMainMenu(lName, "5") == 1)
                {
                    Production.Attributes.Add("class", "hidden");
                }

                if (XERPSecure.HideMainMenu(lName, "6") == 1)
                {
                    Accounts.Attributes.Add("class", "hidden");
                }

                if (XERPSecure.HideMainMenu(lName, "7") == 1)
                {
                    Payroll.Attributes.Add("class", "hidden");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }


    public void LoadMenu()
    {
        if (Page.Request.Path.Contains("Default.aspx"))
        {
            dashboard.Attributes.Add("class", "start active");
        }
        //sales menu
        else if (Page.Request.Path.Contains("Party.aspx"))
        {
            //Party
            string formtype = base.Request.QueryString["type"];
            if (formtype == "customer")
            {
                salesMenu.Attributes.Add("class", "expand");
                Customers.Attributes.Add("class", "xerp_curr");
                Page.Title = "Customer Setup";
                ChkFormAccessUrl("Party.aspx");
            }
            else if (formtype == "supplier")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                supplier.Attributes.Add("class", "xerp_curr");
                Page.Title = "Manufacturer Setup";
            }
            else if (formtype == "agents")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                agents.Attributes.Add("class", "xerp_curr");
                Page.Title = "Import Agents Setup";
            }
            else if (formtype == "cnf")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                cnf.Attributes.Add("class", "xerp_curr");
                Page.Title = "CNF Agents Setup";
            }
            else if (formtype == "transport")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                transport.Attributes.Add("class", "xerp_curr");
                Page.Title = "Transport Agents Setup";
            }
            else if (formtype == "insurance")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                insurance.Attributes.Add("class", "xerp_curr");
                Page.Title = "Insurance Company Setup";
            }
            else if (formtype == "vendor")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                liVendor.Attributes.Add("class", "xerp_curr");
                Page.Title = "Supplier Setup";
            }
            else
            {
                //Response.Redirect("Default.aspx");
            }
        }

        else if (Page.Request.Path.Contains("Receipts-Payment.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            ReceiptsPayment.Attributes.Add("class", "xerp_curr");
            Page.Title = "Receipts & Payment Account";
        }
        else if (Page.Request.Path.Contains("Requisition.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            RequisitionEntry.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Ref-Items.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            Li6.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Customer-Brands.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Brands.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Customer-Brands.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            person_sales.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Item-Types.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            ItemTypes.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Finished-Items.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            menuID63.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Finished-Items2.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            menuID63New.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Zone.aspx"))
        {
            //Zone/Cities
            string formtype = base.Request.QueryString["type"];
            if (formtype == "sales")
            {
                salesMenu.Attributes.Add("class", "expand");
                sales_zone.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "purchase")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                zone_purchase.Attributes.Add("class", "xerp_curr");
            }
        }
        else if (Page.Request.Path.Contains("Customer-Delivery-Points.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            delivery_Points.Attributes.Add("class", "xerp_curr");
        }

        //Sales-Xclusive
        else if (Page.Request.Path.Contains("Order-Entry.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            order.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Order-Edit.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            cancelorder.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Create-PI.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            CreatePI.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Edit-PI.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            EditPI.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LLC-from-PI.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li7.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LLC-Amendment.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li19.Attributes.Add("class", "xerp_curr");
        }

        else if (Page.Request.Path.Contains("Order-Delivery.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            delivery.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sales-Direct.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            directsale.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sales-Fixed-Items.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li58.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("SalesEntryBOQ.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            salesBOQ.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sales-Xclusive.aspx"))
        {
            string formtype = base.Request.QueryString["type"];
            if (formtype == "quotation")
            {
                salesMenu.Attributes.Add("class", "expand");
                //Quotation.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "order")
            {
                salesMenu.Attributes.Add("class", "expand");
                order.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "cancelorder")
            {
                salesMenu.Attributes.Add("class", "expand");
                cancelorder.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "sales")
            {
                salesMenu.Attributes.Add("class", "expand");
                //sales.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "delivery")
            {
                salesMenu.Attributes.Add("class", "expand");
                delivery.Attributes.Add("class", "xerp_curr");
            }
        }
        else if (Page.Request.Path.Contains("Sales-Return.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            SalesReturn.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Order-Close.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            OrderClose.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Collection.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Collection.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("CollectionEdit.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            CollectionEdit.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("CollectionAdjustment.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            CollectionAdjustment.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Collection-LC.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            CollectionLC.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Cheque.aspx"))//
        {
            salesMenu.Attributes.Add("class", "expand");
            Chequesales.Attributes.Add("class", "xerp_curr");
            //Zone/Cities
            string formtype = base.Request.QueryString["type"];
            if (formtype == "sales")
            {
                salesMenu.Attributes.Add("class", "expand");
                Chequesales.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "purchase")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                Chequepurchase.Attributes.Add("class", "xerp_curr");
            }
        }
        else if (Page.Request.Path.Contains("Cheque-Receipt-Payment.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li69.Attributes.Add("class", "xerp_curr");
            Page.Title = "Cheque Recip/Payment";
        }
        else if (Page.Request.Path.Contains("Cheque-Payment.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            Chequepurchase.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Collection-by-Date.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            //CollectionbyDate.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Adjustment.aspx"))
        {
            //Zone/Cities
            string formtype = base.Request.QueryString["type"];
            if (formtype == "sales")
            {
                salesMenu.Attributes.Add("class", "expand");
                //Adjustmentcustomer.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "supplier")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                //Adjustmentsupplier.Attributes.Add("class", "xerp_curr");
            }
        }

        else if (Page.Request.Path.Contains("Sales-Statement.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li48.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("MaturedBillStatement.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li002.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("OutstandingList.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li003.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Item-Report.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            SaleItemReport.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Item-byBrand-Report.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            SalesItembyBrandReport.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Pack-Report.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li23.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Pack-Company.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li28.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Grade-Report.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li24.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Grade-Company.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li25.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sales-Month.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li29.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Year-Sale-Packsize.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li31.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Pack-Year.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li32.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Type-Year.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li33.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Customer-Report.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            SaleCustomerReport.Attributes.Add("class", "xerp_curr");
        }

        else if (Page.Request.Path.Contains("Sale-Details-Report.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li21.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("PO-Date-Range.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li12.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("PO-Items-Report.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li27.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("PO-Status.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li14.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Invoice-Date-Range.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li13.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Invoice-Pay-Status.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li26.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Search-Invoice.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li8.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Customer-Credit-List.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            CustomerCreditList.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Customer-Ladger.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            CustomerLadger.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sales-Collection-History.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            Li30.Attributes.Add("class", "xerp_curr");
        }
        //BOQReports
        else if (Page.Request.Path.Contains("ItemWiseProfitCalculationReport.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            itemWiseProfit.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("InvoiceWiseProfitCalculation.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            invoiceWiseProfit.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("FifoSummaryReport.aspx"))
        {
            salesMenu.Attributes.Add("class", "expand");
            fifoSummary.Attributes.Add("class", "xerp_curr");
        }

        else if (Page.Request.Path.Contains("Sales-Persons.aspx"))
        {
            //Bank.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "sr")
            {
                salesMenu.Attributes.Add("class", "expand");
                person_sales.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "ref")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                person_ref.Attributes.Add("class", "xerp_curr");
            }
        }




        else if (Page.Request.Path.Contains("Collection.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            Collection.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Collection.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            Collection.Attributes.Add("class", "xerp_curr");
        }


        else if (Page.Request.Path.Contains("Bank.aspx"))
        {
            //Bank.aspx
            string formtype = base.Request.QueryString["type"];
            //if (formtype == "insurance")
            //{
            //    purchaseMenu.Attributes.Add("class", "expand");
            //    insurance.Attributes.Add("class", "xerp_curr");
            //}
            if (formtype == "bank")
            {
                Accounts.Attributes.Add("class", "expand");
                bank.Attributes.Add("class", "xerp_curr");
            }
        }
        else if (Page.Request.Path.Contains("Exp-Types.aspx"))
        {
            //ExpHead.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "lc")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                menuID37.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "crm")
            {
                CRM.Attributes.Add("class", "expand");
                ExpHeadcrm.Attributes.Add("class", "xerp_curr");
            }
        }

        else if (Page.Request.Path.Contains("ExpHead.aspx"))
        {
            //ExpHead.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "lc")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                lc.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "crm")
            {
                CRM.Attributes.Add("class", "expand");
                ExpHeadcrm.Attributes.Add("class", "xerp_curr");
            }
        }




        //Purchase Setup

        else if (Page.Request.Path.Contains("Party2.aspx"))
        {
            //Party
            string formtype = base.Request.QueryString["type"];
            if (formtype == "customer")
            {
                salesMenu.Attributes.Add("class", "expand");
                Customers.Attributes.Add("class", "xerp_curr");
                Page.Title = "Customer Setup";
            }
            else if (formtype == "supplier")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                supplier.Attributes.Add("class", "xerp_curr");
                Page.Title = "Manufacturer Setup";
            }
            else if (formtype == "agents")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                agents.Attributes.Add("class", "xerp_curr");
                Page.Title = "Import Agents Setup";
            }
            else if (formtype == "cnf")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                cnf.Attributes.Add("class", "xerp_curr");
                Page.Title = "CNF Agent Setup";
            }
            else if (formtype == "transport")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                transport.Attributes.Add("class", "xerp_curr");
                Page.Title = "Transport Agent Setup";
            }
            else if (formtype == "insurance")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                insurance.Attributes.Add("class", "xerp_curr");
                Page.Title = "Insurance Company Setup";
            }
            else if (formtype == "vendor")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                liVendor.Attributes.Add("class", "xerp_curr");
                Page.Title = "Supplier Setup";
            }
            else
            {
                //Response.Redirect("Default.aspx");
            }
        }


        else if (Page.Request.Path.Contains("Purchase.aspx"))
        {
            //Purchase.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "req")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                //Purchase_req.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "reqApproval")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                //PurchasereqApproval.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "purchase")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                Purchasepurchase.Attributes.Add("class", "xerp_curr");
                Page.Title = "Purchase Entry";
            }
            if (formtype == "edit")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                PurchaseEdit.Attributes.Add("class", "xerp_curr");
                Page.Title = "Purchase Edit";
            }
        }

        else if (Page.Request.Path.Contains("Purchase-Return.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            PurchaseReturn.Attributes.Add("class", "xerp_curr");
        }

        else if (Page.Request.Path.Contains("LC-Open.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            //LCOpenCreate.Attributes.Add("class", "xerp_curr");
            //LC-Open.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "create")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                LCOpenCreate.Attributes.Add("class", "xerp_curr");
                Page.Title = "LC General Information";
            }
            if (formtype == "edit")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                LCOpenEdit.Attributes.Add("class", "xerp_curr");
                Page.Title = "LC Amendment";
            }
        }
        else if (Page.Request.Path.Contains("Status-Updates.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            string formtype = base.Request.QueryString["type"];
            if (formtype == "LC")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                statusLc.Attributes.Add("class", "xerp_curr");
                Page.Title = "LC Status Updates";
            }
            if (formtype == "edit")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                LCOpenEdit.Attributes.Add("class", "xerp_curr");
                Page.Title = "LC Amendment";
            }
        }

        else if (Page.Request.Path.Contains("LC-Preview.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            menuID57.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LC-Expenses.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            LCExpenses.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LC-Finalization.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            LCFinalization.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LC-Return.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            LCReturn.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LC-Items.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            LCVoucher.Attributes.Add("class", "xerp_curr");
        }
        //else if (Page.Request.Path.Contains("LC-Items.aspx"))
        //{
        //    purchaseMenu.Attributes.Add("class", "expand");
        //    LCVoucher.Attributes.Add("class", "xerp_curr");
        //}

        else if (Page.Request.Path.Contains("Payment.aspx"))
        {
            //Payment.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "local")
            {
                purchaseMenu.Attributes.Add("class", "expand");
                Paymentlocal.Attributes.Add("class", "xerp_curr");

            }
            //if (formtype == "lc")
            //{
            //    purchaseMenu.Attributes.Add("class", "expand");
            //    Paymentlc.Attributes.Add("class", "xerp_curr");
            //}
        }
        else if (Page.Request.Path.Contains("Currencies.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            Currencies.Attributes.Add("class", "xerp_curr");
        }

        // Purchase Reports
        else if (Page.Request.Path.Contains("RPT-Purchase-Invoice.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            menuID49.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Supplier-Ladger.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            menuID50.Attributes.Add("class", "xerp_curr");
        }

        //else if (Page.Request.Path.Contains("Supplier-Ladger1.aspx"))
        //{
        //    purchaseMenu.Attributes.Add("class", "expand");
        //    Li68.Attributes.Add("class", "xerp_curr");
        //}
        else if (Page.Request.Path.Contains("Item-Purchase-History.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            Li52.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Item-Wise-Purchase-Report.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            ItemWisePurchaseReport.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Item-Purchase-History2.aspx"))
        {

            purchaseMenu.Attributes.Add("class", "expand");
            //Li52P1.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Supplier-Credit-List.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            menuID52.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Categories.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            //SaleCategories.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Types-Rpt.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            //SaleTypesRpt.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("DO-Rpt.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            //DORpt.Attributes.Add("class", "xerp_curr");
        }

        // LC Reports
        else if (Page.Request.Path.Contains("LC-Date-Range.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            Li20.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LC-ExpenseReport.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            lcExpenseRpt.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LC-ExpenseSummary.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            lcExpenseSummary.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LC-ExpenseDetails.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            lcExpenseDetails.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Party-Ledger.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            partyLedger.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Item-Costing-Comparison.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            menuID56.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LC-Items-History.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            menuID54.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sale-Types-Rpt.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            //SaleTypesRpt.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("DO-Rpt.aspx"))
        {
            purchaseMenu.Attributes.Add("class", "expand");
            //DORpt.Attributes.Add("class", "xerp_curr");
        }

        //Inventory

        else if (Page.Request.Path.Contains("Item-Group.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            ItemGroup.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Item-Subgrp.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            ItemSubgrp.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Item-Grade.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            ItemGrade.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Item-Category.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            ItemCategory.Attributes.Add("class", "xerp_curr");
        }

        else if (Page.Request.Path.Contains("Products.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Products.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Item-Unit.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            ItemUnit.Attributes.Add("class", "xerp_curr");
        }


        else if (Page.Request.Path.Contains("Warehouses.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Warehouses.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("SisterConcernWarehouseSetup.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            sisterConcernWarehouse.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Warehouse-Areas.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            WarehouseAreas.Attributes.Add("class", "xerp_curr");
        }


        else if (Page.Request.Path.Contains("Store-Raw.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li63.Attributes.Add("class", "xerp_curr");
            Page.Title = "Store activities (Issue/Receive)";
        }
        else if (Page.Request.Path.Contains("InkIssue.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            inkIssue.Attributes.Add("class", "xerp_curr");
            Page.Title = "Ink Issue";
        }
        else if (Page.Request.Path.Contains("StockLoanIssueReceive.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            stockLoanTakenGiven.Attributes.Add("class", "xerp_curr");
            Page.Title = "Stock Loan Issue Receive";
        }
        else if (Page.Request.Path.Contains("Crushing.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li9.Attributes.Add("class", "xerp_curr");
            Page.Title = "Store activities (Crushing)";
        }
        else if (Page.Request.Path.Contains("RequisitionIssue.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li80.Attributes.Add("class", "xerp_curr");
            Page.Title = "Requisition Issue Entry";
        }
        else if (Page.Request.Path.Contains("RequisitionApproval.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li81.Attributes.Add("class", "xerp_curr");
            Page.Title = "Requisition Approval";
        }
        else if (Page.Request.Path.Contains("Stock-Adj-Raw.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li1.Attributes.Add("class", "xerp_curr");
            Page.Title = "Raw Stock Adjustment";
        }

        else if (Page.Request.Path.Contains("Stock-Adj-Waste.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li3.Attributes.Add("class", "xerp_curr");
            Page.Title = "Waste Stock Adjustment";
        }

        else if (Page.Request.Path.Contains("Stock-Adj-Processed.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li4.Attributes.Add("class", "xerp_curr");
            Page.Title = "Processed Stock Adjustment";
        }
        else if (Page.Request.Path.Contains("Stock-Adj-Finished.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li2.Attributes.Add("class", "xerp_curr");
            Page.Title = "Finished Stock Adjustment";
        }
        else if (Page.Request.Path.Contains("Stock-Adj-Machine.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li5.Attributes.Add("class", "xerp_curr");
            Page.Title = "Machinery Stock Adjustment";
        }
        else if (Page.Request.Path.Contains("Stock-Adj-Others.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li5x.Attributes.Add("class", "xerp_curr");
            Page.Title = "Others Stock Adjustment";
        }

        else if (Page.Request.Path.Contains("Stock-Transfer.aspx"))
        {

            string formtype = base.Request.QueryString["type"];
            if (formtype == "raw")
            {
                Inventory.Attributes.Add("class", "expand");
                StockTransferraw.Attributes.Add("class", "xerp_curr");
            }
            /*           if (formtype == "finished")
                       {
                           Inventory.Attributes.Add("class", "expand");
                           StockTransferfinished.Attributes.Add("class", "xerp_curr");
                       }
                       if (formtype == "rdamaged")
                       {
                           Inventory.Attributes.Add("class", "expand");
                           StockTransferrdamaged.Attributes.Add("class", "xerp_curr");
                       }
                       if (formtype == "fdamaged")
                       {
                           Inventory.Attributes.Add("class", "expand");
                           StockTransferfdamaged.Attributes.Add("class", "xerp_curr");
                       } */
        }
        else if (Page.Request.Path.Contains("MachineToMachineTransfer.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            MachineStockTransfer.Attributes.Add("class", "xerp_curr");
            Page.Title = "Machine Stock Transfer";
        }
        //else if (Page.Request.Path.Contains("Stock-Damaged.aspx"))
        //{
        //    Inventory.Attributes.Add("class", "expand");
        //    StockDamaged.Attributes.Add("class", "xerp_curr");
        //}


        else if (Page.Request.Path.Contains("Current-Stock-Raw.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            CurrentStock.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("MachineStockRaw.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            MachineStock.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("CategoryWiseStock.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            StockCategory.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("CategoryWiseStockValue.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            StockCategoryValue.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("GradeWiseStock.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            StockGrade.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("GradeWiseStockValue.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            StockGradeValue.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("StockLedger.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            StockLedger.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Current-Stock-Wastage.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li34.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Current-Stock-Processed.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li35.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Current-Stock-Finished.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li36.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Current-Stock-Machineries.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li37.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Current-Stock-Others.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li38.Attributes.Add("class", "xerp_curr");
        }

        else if (Page.Request.Path.Contains("Stock-In-Out.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            StockInOut.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Stock-Transfer-History.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            StockTransferHistory.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("FIFO-List.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li65.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("RMConsumptionReport.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            RMConsumption.Attributes.Add("class", "xerp_curr");
        }
        //else if (Page.Request.Path.Contains("WastageStockConsumption.aspx"))
        //{
        //    Inventory.Attributes.Add("class", "expand");
        //    WastageStockConsumption.Attributes.Add("class", "xerp_curr");
        //}
        else if (Page.Request.Path.Contains("RawStockIssueReport.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            RawStockIssueReport.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("RawStockApprovalReport.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            Li74.Attributes.Add("class", "xerp_curr");
        }

        //Inventory New
        /* else if (Page.Request.Path.Contains("StockGroup.aspx"))
         {
             InventoryNew.Attributes.Add("class", "expand");
             StockGroup.Attributes.Add("class", "xerp_curr");
         }
         else if (Page.Request.Path.Contains("StockSubsidiary.aspx"))
         {
             InventoryNew.Attributes.Add("class", "expand");
             StockSubsidiary.Attributes.Add("class", "xerp_curr");
         }
         else if (Page.Request.Path.Contains("StockControl.aspx"))
         {
             InventoryNew.Attributes.Add("class", "expand");
             StockControl.Attributes.Add("class", "xerp_curr");
         }
         else if (Page.Request.Path.Contains("StockHeadSetup.aspx"))
         {
             InventoryNew.Attributes.Add("class", "expand");
             StockHeadSetup.Attributes.Add("class", "xerp_curr");
         }
         */
        else if (Page.Request.Path.Contains("Production-AccountsLinkup.aspx"))
        {
            InventoryNew.Attributes.Add("class", "expand");
            ProductionACLinkup.Attributes.Add("class", "xerp_curr");
        }

        else if (Page.Request.Path.Contains("StockTransaction.aspx"))
        {
            InventoryNew.Attributes.Add("class", "expand");
            StockTransaction.Attributes.Add("class", "xerp_curr");
        }
        //Inventory New (Reports)
        else if (Page.Request.Path.Contains("StockHeadLedger.aspx"))
        {
            InventoryNew.Attributes.Add("class", "expand");
            StockHeadLedger.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("ControlStockSummary.aspx"))
        {
            InventoryNew.Attributes.Add("class", "expand");
            ControlStockSummary.Attributes.Add("class", "xerp_curr");
        }
        //else if (Page.Request.Path.Contains("FinishedStockLedger.aspx"))
        //{
        //    InventoryNew.Attributes.Add("class", "expand");
        //    FinishedStockLedger.Attributes.Add("class", "xerp_curr");
        //}
        ///Production
        else if (Page.Request.Path.Contains("Production-Sections.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            ProductionSections.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Production-Machines.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Li46.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Production-Stages.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            ProductionStages.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-Effeciency.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            EmployeeEffeciency.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("SemiFinishedPackSizeSetup.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            semiFinishedPackSizeSetup.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("PolyFinishedGoodsSizeSetup.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            polyFinishedSize.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Production-Section.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            ProductionSection.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("FormRawStockConsumption.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            RawStockConsumption.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("WastageStockRegister.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            wastageStockRegister.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Wastage Stock Register";
        }
        else if (Page.Request.Path.Contains("Production-Shift.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Shift.Attributes.Add("class", "xerp_curr");
        }



        else if (Page.Request.Path.Contains("Raw-Consumptions.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            //RawConsumptions.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Packaging-Consumptions.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            //PackagingConsumptions.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-Production.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            //EmployeeProduction.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Production-Purpose.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            purpose.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Production-Standard.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            prdStandard.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Ink-Specifications.aspx"))
        {
            Inventory.Attributes.Add("class", "expand");
            InkSpecifications.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("MouldInfo.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            mould.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("PolyDimension.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            polyDimension.Attributes.Add("class", "xerp_curr");
        }
        //------------------ Production--------------------------
        else if (Page.Request.Path.Contains("Cutting-Shearing.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Cutting.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Shearing";
        }
        else if (Page.Request.Path.Contains("Power-Press.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Power.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Power Press";
        }
        else if (Page.Request.Path.Contains("Grinding.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            //Grinding.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Grinding";
        }
        else if (Page.Request.Path.Contains("Offset-Printing.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Offset.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Offset Printing";
        }
        else if (Page.Request.Path.Contains("Round-Tin.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Round.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Round Tin";
        }
        else if (Page.Request.Path.Contains("rpt-Daily-Production.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Li64.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("rpt-Prdn-Plastic-Container.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Li52pc.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("DeliveryReport.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            DeliveryReport.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("ProductionInput.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            RawMaterial.Attributes.Add("class", "xerp_curr");

        }
        else if (Page.Request.Path.Contains("ProductionOutput.aspx"))
        {
            if (!string.IsNullOrEmpty(Request.QueryString["p"]))//IML
            {
                Production.Attributes.Add("class", "expand");
                Li50.Attributes.Add("class", "xerp_curr");
            }
            else
            {
                Production.Attributes.Add("class", "expand");
                ProducedItem.Attributes.Add("class", "xerp_curr");
                Page.Title = "Production: Produced Item";
            }
        }
        else if (Page.Request.Path.Contains("Screen-Print.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Screen.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Screen Printing";
        }
        else if (Page.Request.Path.Contains("DOPSection.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            dopSection.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: DOP Section";
        }
        else if (Page.Request.Path.Contains("IML-Output.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Li50.Attributes.Add("class", "xerp_curr");
            //Page.Title = "Production: Screen Printing";
        }
        else if (Page.Request.Path.Contains("HTF-Print.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Li51.Attributes.Add("class", "xerp_curr");
            //Page.Title = "Production: Screen Printing";
        }
        else if (Page.Request.Path.Contains("InkStock.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            inkStock.Attributes.Add("class", "xerp_curr");
            //Page.Title = "Production: Screen Printing";
        }
        else if (Page.Request.Path.Contains("Plastic-Finishing.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Plastic2.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Plastic Finishing";
        }
        else if (Page.Request.Path.Contains("Poly-Section.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            polySection.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Poly Section";
        }
        //------------------ End Production--------------------------
        //----------------------------Start Crushing----------------------
        else if (Page.Request.Path.Contains("CrushingSection.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            crushingSection.Attributes.Add("class", "xerp_curr");
            Page.Title = "Production: Crushing Section";
        }

        //---------------- End Crushing ------------------------------
        else if (Page.Request.Path.Contains("Stock-in.aspx"))
        {
            //Stock-in.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "finished")
            {
                Production.Attributes.Add("class", "expand");
                //Stockinfinished.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "packaging")
            {
                Production.Attributes.Add("class", "expand");
                //Stockinpackaging.Attributes.Add("class", "xerp_curr");
            }
        }
        else if (Page.Request.Path.Contains("Stock-Out.aspx"))
        {
            //Stock-Out.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "raw")
            {
                Inventory.Attributes.Add("class", "expand");
                //Stockoutraw.Attributes.Add("class", "xerp_curr");
            }
        }

        else if (Page.Request.Path.Contains("Stock-Register.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            StockRegister.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("InkStockRegister.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            inkStockSummary.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("StockRegisterForHTFFoils.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            stockRegisterForHTFFoils.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("StockRegisterForIMLItems.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            stockRegisterForIMLItems.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Production-Hourly.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            ProductionHourly.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Production-Efficiency.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            ProductionEfficiency.Attributes.Add("class", "xerp_curr");
        }

        //---- prd Rpt ==

        else if (Page.Request.Path.Contains("Production-Date.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            ProductionDate.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("rpt-Prdn-HTF.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Li52htf.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("rpt-Prdn-Plastic-Finish.aspx"))
        {
            Production.Attributes.Add("class", "expand");
            Li52pf.Attributes.Add("class", "xerp_curr");
        }
        //---- End prd Rpt ==


        else if (Page.Request.Path.Contains("Accounts-Group.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            //AccountsGroup.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("SubAcc.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            AccountsCategory.Attributes.Add("class", "xerp_curr");
            Page.Title = "Subsidiary Accounts";
        }
        else if (Page.Request.Path.Contains("LedgerControl.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            LedgerControl.Attributes.Add("class", "xerp_curr");
            Page.Title = "Control Accounts Ledger";
        }
        else if (Page.Request.Path.Contains("Control.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            AccountsControl.Attributes.Add("class", "xerp_curr");
            Page.Title = "Control Accounts";
        }
        else if (Page.Request.Path.Contains("LedgerHead.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            LedgerHead.Attributes.Add("class", "xerp_curr");
            Page.Title = "Accounts Head Ledger";
        }
        else if (Page.Request.Path.Contains("LedgerHead.aspx?type=Qty"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li666.Attributes.Add("class", "xerp_curr");
            Page.Title = "Accounts Head Ledger (Qty)";
        }

        else if (Page.Request.Path.Contains("Sale-Colln-Summery.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li54.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Acc-Trade-Receivable.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            accTrRec.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Acc-Trade-Payable.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li66.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("BankLoansReport.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li68.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("LoanLedger.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li70.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Head.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            AccountsHead.Attributes.Add("class", "xerp_curr");
            Page.Title = "Accounts Head";
        }
        else if (Page.Request.Path.Contains("Bank-Accounts.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            BankAccounts.Attributes.Add("class", "xerp_curr");
            Page.Title = "Bank Accounts";
        }
        else if (Page.Request.Path.Contains("Particulars.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Particulars.Attributes.Add("class", "xerp_curr");
            Page.Title = "Particulars";
        }
        else if (Page.Request.Path.Contains("Acc-Rpt-Settings.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            //AccRptSettings.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("AddFixedAssets.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            AccFixedAssetsSch.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("BankLoanTypes.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            BankLoanTypes.Attributes.Add("class", "xerp_curr");
            Page.Title = "BankLoan Types";
        }
        else if (Page.Request.Path.Contains("BankLoan.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            BankLoan.Attributes.Add("class", "xerp_curr");
            Page.Title = "Bank Loans";
        }
        else if (Page.Request.Path.Contains("BankBranch.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            bankBranch.Attributes.Add("class", "xerp_curr");
            Page.Title = "Branch Setup";
        }
        else if (Page.Request.Path.Contains("ShareholdersEquity.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            ShareholdersEquity.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("FixedAssetsVoucherEntry.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            FixedAssetsVoucherEntry.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("VoucherEntry.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            VoucherEntry.Attributes.Add("class", "xerp_curr");
            Page.Title = "Vouchers Entry";
        }
        else if (Page.Request.Path.Contains("LC-VoucherApproval.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li62.Attributes.Add("class", "xerp_curr");
            Page.Title = "Vouchers Entry";
        }
        else if (Page.Request.Path.Contains("Finyearprocess.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            lcVoucherApproval.Attributes.Add("class", "xerp_curr");
            Page.Title = "Cash Flow Financial Year Process";
        }
        else if (Page.Request.Path.Contains("Voucher-Qty.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li61.Attributes.Add("class", "xerp_curr");
            //Page.Title = "Voucher Edit/Delete";
        }
        else if (Page.Request.Path.Contains("VoucherCancel.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            //VoucherCancel.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("AccLink.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            //CashTransfer.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Expenses.aspx"))
        {
            //Stock-Out.aspx
            string formtype = base.Request.QueryString["type"];
            if (formtype == "acc")
            {
                Accounts.Attributes.Add("class", "expand");
                //ExpensesAcc.Attributes.Add("class", "xerp_curr");
            }
            if (formtype == "crm")
            {
                CRM.Attributes.Add("class", "expand");
                Expensescrm.Attributes.Add("class", "xerp_curr");
            }
        }



        else if (Page.Request.Path.Contains("Accounts-Chart-Report.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            AccountsChartReport.Attributes.Add("class", "xerp_curr");
            Page.Title = "Chart of Accounts";
        }
        else if (Page.Request.Path.Contains("Voucher-Edit-History.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            VoucherEditHistory.Attributes.Add("class", "xerp_curr");
            Page.Title = "Voucher Edit History";
        }

        else if (Page.Request.Path.Contains("Cash-Statement.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            //CashStatement.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Bank-Ladger.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            BankBook.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Bank-Book.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            BankBalanceReport.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Trial-Balance.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            TrialBalance.Attributes.Add("class", "xerp_curr");
            Page.Title = "Trial Balance";
        }
        else if (Page.Request.Path.Contains("PL.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            //PL.Attributes.Add("class", "xerp_curr");
            Page.Title = "Chart of Accounts";
        }
        else if (Page.Request.Path.Contains("IncomeStatement.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            IncomeStatement.Attributes.Add("class", "xerp_curr");
            Page.Title = "Income Statement";
        }

        else if (Page.Request.Path.Contains("BalanceSheet.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            BalanceSheet.Attributes.Add("class", "xerp_curr");
            Page.Title = "Balance Sheet";
        }
        else if (Page.Request.Path.Contains("CashFlow.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            CashFlow.Attributes.Add("class", "xerp_curr");
            Page.Title = "Cash Flow Statement";
        }

        //else if (Page.Request.Path.Contains("equity-ratio.aspx"))
        //{
        //    Accounts.Attributes.Add("class", "expand");
        //    equityratio.Attributes.Add("class", "xerp_curr");
        //    Page.Title = "Equity Ratio";
        //}
        else if (Page.Request.Path.Contains("LedgerSub.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            LedgerSub.Attributes.Add("class", "xerp_curr");
            Page.Title = "Subsidiary Accounts Ledger";
        }
        else if (Page.Request.Path.Contains("PendingVouchers.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            //PendingVouchers.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("CancelledVouchers.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            CancelledVouchers.Attributes.Add("class", "xerp_curr");
            Page.Title = "Chart of Accounts";
        }
        else if (Page.Request.Path.Contains("ALLEntries.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            ALLEntries.Attributes.Add("class", "xerp_curr");
            Page.Title = "Chart of Accounts";
        }

        else if (Page.Request.Path.Contains("FixedAssetsOut.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li710.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("FixedAssetsRegister.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li711.Attributes.Add("class", "xerp_curr");
            Page.Title = "Fixed Assets Register";
        }
        else if (Page.Request.Path.Contains("FixedAssetPosition.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li72.Attributes.Add("class", "xerp_curr");
            Page.Title = "Fixed Assets Position";
        }
        else if (Page.Request.Path.Contains("FixedAssetLedger.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li71.Attributes.Add("class", "xerp_curr");
            Page.Title = "Fixed Assets Ledger";
        }
        else if (Page.Request.Path.Contains("DepreciationSummaryReport.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li75.Attributes.Add("class", "xerp_curr");
            Page.Title = "Depreciation Summary Report";
        }
        else if (Page.Request.Path.Contains("DepRepo.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li73.Attributes.Add("class", "xerp_curr");
            Page.Title = "Fixed Assets Depreciation";
        }
        else if (Page.Request.Path.Contains("TDS-Report.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li55.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Changes-in-Equity.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            ChangesinEquity.Attributes.Add("class", "xerp_curr");
            Page.Title = "Changes in Equity";
        }

        else if (Page.Request.Path.Contains("Control-Balance.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li55x.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Profit-Month.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li56.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Qty-Date-Ledger.aspx"))
        {
            Accounts.Attributes.Add("class", "expand");
            Li60.Attributes.Add("class", "xerp_curr");
        }


        else if (Page.Request.Path.Contains("Employee-Department.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmployeeDepartment.Attributes.Add("class", "xerp_curr");
        }

        else if (Page.Request.Path.Contains("Employee-Sectionas.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            Sections.Attributes.Add("class", "xerp_curr");
            Page.Title = "Employee Sections";
        }
        else if (Page.Request.Path.Contains("Emp-Type.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            emptype.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("hrm-shift.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            workSlots.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Holidays.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            Li45.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-Designation.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmployeeDesignation.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-Info.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmployeeInfo.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-Close.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmployeeClose.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-Bonus.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmployeeBonus.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Daily-Attn.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpDailyAttn.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Edit-Attn.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpEditAttn.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Monthly-Attn.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpMonthlyAttn.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Salary-Process.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpSalaryProcess.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Bonus-Process.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpBonusProcess.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Payment.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpPayment.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-List-Report.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmployeeListReport.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-Salary-Report.aspx"))
        {
            EmployeeSalaryReport.Attributes.Add("class", "expand");
            Warehouses.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Employee-PaySlip-Report.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmployeePaySlipReport.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Ladger.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpLadger.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Att-History.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpAttHistory.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Emp-Applications.aspx"))
        {
            Payroll.Attributes.Add("class", "expand");
            EmpApplications.Attributes.Add("class", "xerp_curr");
        }


        else if (Page.Request.Path.Contains("Document-Library.aspx"))
        {
            CRM.Attributes.Add("class", "expand");

            string formtype = base.Request.QueryString["type"];
            if (formtype == "LC")
            {
                Li39.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "PO")
            {
                Li40.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "Purchase")
            {
                Li41.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "LLC")
            {
                Li42.Attributes.Add("class", "xerp_curr");
            }
            else if (formtype == "Others")
            {
                Li43.Attributes.Add("class", "xerp_curr");
            }
            else
            {
                Li44.Attributes.Add("class", "xerp_curr");
            }
        }


        else if (Page.Request.Path.Contains("Address-Book.aspx"))
        {
            CRM.Attributes.Add("class", "expand");
            AddressBook.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Task-Scheduler.aspx"))
        {
            CRM.Attributes.Add("class", "expand");
            TaskScheduler.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Complain-Catagories.aspx"))
        {
            CRM.Attributes.Add("class", "expand");
            ComplainCatagories.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Customer-Complains.aspx"))
        {
            CRM.Attributes.Add("class", "expand");
            CustomerComplains.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Support-Ticket.aspx"))
        {
            CRM.Attributes.Add("class", "expand");
            SupportTicket.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Send-Email.aspx"))
        {
            CRM.Attributes.Add("class", "expand");
            SendEmail.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Fact-Price-Declaration.aspx"))
        {
            FactsModule.Attributes.Add("class", "expand");
            Li10.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Fact-Consum-Stat.aspx"))
        {
            FactsModule.Attributes.Add("class", "expand");
            Li15.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Fact-Print-Impresion.aspx"))
        {
            FactsModule.Attributes.Add("class", "expand");
            Li17.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Fact-Reduce-MTT-Thiner.aspx"))
        {
            FactsModule.Attributes.Add("class", "expand");
            Li16.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Fact-Ink-Consum.aspx"))
        {
            FactsModule.Attributes.Add("class", "expand");
            Li18.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Fact-Coating-Vernish-Sizeetc.aspx"))
        {
            FactsModule.Attributes.Add("class", "expand");
            Li11.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Fact-Tin-Sheet.aspx"))
        {
            FactsModule.Attributes.Add("class", "expand");
            menuID141.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Fact-Height-Spec.aspx"))
        {
            FactsModule.Attributes.Add("class", "expand");
            menuID142.Attributes.Add("class", "xerp_curr");
        }


        else if (Page.Request.Path.Contains("Country_setup.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Country_setup.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Setup_City.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Setup_City.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Sub-Menu.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Li49.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Company.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Company.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Settings-Forms.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            SettingsForms.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Post-News.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            PostNews.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Post-Message.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            PostMessage.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("NewsEdit.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            NewsEdit.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("All-News.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            AllNews.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Check-Msg.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            CheckMsg.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Permission-Level.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Li47.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("FormUserLevelSecurity.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Li57.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("MenuStructure.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            MenuStructure.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("System-Settings.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Li53.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("New-User.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            NewUser.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Settings-User-Permission.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            SettingsUserPermission.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Unlock.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Unlock.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("ResetPassword.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            ResetPassword.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Login-History.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            LoginHistory.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("Documents-Upload.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            DocumentsUpload.Attributes.Add("class", "xerp_curr");
        }

        //Admin: Maintenance
        else if (Page.Request.Path.Contains("Backup.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            Li22.Attributes.Add("class", "xerp_curr");
        }
        else if (Page.Request.Path.Contains("FinishedGoodsAdjustProcess.aspx"))
        {
            Admin.Attributes.Add("class", "expand");
            FinishedGoodsAdjustProcess.Attributes.Add("class", "xerp_curr");
        }




        ChkMenuPermissions();
        ChkPermissionss();//For User wise form level security
    }

    private void ChkMenuPermissions()
    {
        if (IsBlocked("SALES", salesMenu) != "1")
        {
            IsBlocked("SALES1", sub1);
            IsBlocked("SALES2", sub2);
            IsBlocked("SALES3", sub3);
            IsBlocked("SalesBOQReports", saleBOQReportsMenu);
            IsBlocked("SALES4", sub4);
        }
        if (IsBlocked("PURCHASE", purchaseMenu) != "1")
        {
            IsBlocked("PURCHASE1", sub5);
            IsBlocked("PURCHASE2", sub6);
            IsBlocked("PURCHASE3", sub7);
            IsBlocked("PURCHASE4", sub8);
            IsBlocked("PURCHASE5", sub9);
        }
        if (IsBlocked("INVENTORY", Inventory) != "1")
        {
            IsBlocked("INVENTORY1", sub10);
            IsBlocked("INVENTORY2", sub11);
            IsBlocked("INVENTORY3", sub12);
            IsBlocked("INVENTORY4", sub13);
            IsBlocked("INVENTORY5", sub14);
        }
        if (IsBlocked("INVENTORY NEW", InventoryNew) != "1")
        {
            IsBlocked("INVENTORYNEW1", SpnId1);
            IsBlocked("INVENTORYNEW2", SpnId2);
            //isBlocked("INVENTORY3", sub12);
            //isBlocked("INVENTORY4", sub13);
            //isBlocked("INVENTORY5", sub14);
        }

        if (IsBlocked("PRODUCTION", Production) != "1")
        {
            IsBlocked("PRODUCTION1", sub15);
            IsBlocked("PRODUCTION2", sub16);
            IsBlocked("PRODUCTION3", sub16a);
            //isBlocked("SalesSub4", sub17);
        }
        if (IsBlocked("ACCOUNTS", Accounts) != "1")
        {
            IsBlocked("ACCOUNTS1", sub17);
            IsBlocked("ACCOUNTS2", sub18);
            IsBlocked("ACCOUNTS3", sub19);
        }
        if (IsBlocked("PAYROLL", Payroll) != "1")
        {
            IsBlocked("PAYROLL1", sub20);
            IsBlocked("PAYROLL2", sub21);
            IsBlocked("PAYROLL3", sub22);
        }
        if (IsBlocked("CRM", CRM) != "1")
        {
            IsBlocked("CRM1", sub23);
            IsBlocked("CR2", sub24);
            IsBlocked("CR3", sub25);
        }
        if (IsBlocked("FACTS", FactsModule) != "1")
        {
            IsBlocked("FACTS1", sub26);
            IsBlocked("FACTS2", sub27);
            IsBlocked("FACTS3", sub28);
        }
        if (IsBlocked("ADMIN", Admin) != "1")
        {
            IsBlocked("ADMIN1", sub29);
            IsBlocked("ADMIN2", sub30);
            IsBlocked("ADMIN3", sub31);
            IsBlocked("ADMIN4", sub32);
            IsBlocked("ADMIN5", sub33);
        }

        //////// Menu Items Check for hiding///// Acc Report Form level security old code
        //IsMenuItemBlocked("Chart of Accounts", AccountsChartReport);
        //IsMenuItemBlocked("Income Statement", IncomeStatement);
        //IsMenuItemBlocked("Trial Balance", TrialBalance);
        //IsMenuItemBlocked("Balance Sheet", BalanceSheet);
        //IsMenuItemBlocked("Bank Book", BankBook);
        //IsMenuItemBlocked("Sub A/C Ledger", LedgerSub);
        //IsMenuItemBlocked("Control A/C Ledger", LedgerControl);
        //IsMenuItemBlocked("A/C Head Ledger", LedgerHead);
        //IsMenuItemBlocked("Trade Receivables", accTrRec);
        //IsMenuItemBlocked("Collection Summery", Li54);
        //IsMenuItemBlocked("Control A/C Summery", Li55);
        //IsMenuItemBlocked("Control A/C Balance", Li55x);
        //IsMenuItemBlocked("Profit by Month", Li56);
        //IsMenuItemBlocked("Cancelled Vouchers", CancelledVouchers);
        //IsMenuItemBlocked("Voucher by Date", ALLEntries);

    }

    private void ChkFormAccessUrl(string formName)
    {
        string user = Page.User.Identity.Name;
        string isBlockEd2 = SQLQuery.ReturnString("Select IsBlocked from FormAccessSecurity where MenuItemID IN(Select sl from MenuStructure where PageName='" + formName.Trim() + "') AND UserID='" + user + "'");
        if (isBlockEd2 == "0")
        {
            Response.Redirect("Default.aspx", true);
        }
    }
    private string IsBlocked(string formName, HtmlGenericControl ctrl)
    {
        string user = Page.User.Identity.Name;
        string isBlockEd2 = SQLQuery.ReturnString("Select IsBlocked from UserForms where UserID='" + user + "' AND  FormName='" + formName + "'");
        if (isBlockEd2 == "1")
        {
            ctrl.Attributes.Remove("class");
            ctrl.Attributes.Add("class", "hidden");
            //ctrl.Attributes.Add("Visible", "False");
        }
        return isBlockEd2;
    }

    /*
    private string IsMenuItemBlocked(string formName, HtmlGenericControl ctrl)
    {
        string user = Page.User.Identity.Name;
        string isBlockEd2 = SQLQuery.ReturnString("Select IsBlocked from MenuPermission where UserID='" + user + "' AND  FormName='" + formName + "'");
        if (isBlockEd2 == "1")
        {
            ctrl.Attributes.Remove("class");
            ctrl.Attributes.Add("class", "hidden");
        }
        return isBlockEd2;
    }*/
    private void ChkPermissionss()
    {
        if (IsBlocked("Sales", salesMenu) != "1")
        {
            IsBlocked("Setups", sub1);
            IsBlocked("Point Of Sales", sub2);
            IsBlocked("Collections", sub3);
            IsBlocked("SalesBOQReports", saleBOQReportsMenu);
            IsBlocked("Reports", sub4);
        }
        if (IsBlocked("Purchase", purchaseMenu) != "1")
        {
            IsBlocked("Requisition", requisition);
            IsBlocked("Setups", sub5);
            IsBlocked("Local Purchase", sub6);
            IsBlocked("Purchase Reports", sub8);
        }
        if (IsBlocked("Inventory", Inventory) != "1")
        {
            IsBlocked("Product Setup", sub10);
            /*isBlocked("INVENTORY3", CheckBox14);
            isBlocked("INVENTORY4", CheckBox15);
            isBlocked("INVENTORY5", CheckBox135);*/
        }
        if (IsBlocked("Inventory New", InventoryNew) != "1")
        {
            IsBlocked("Stock Transaction", SpnId1);
            IsBlocked("Stock Head Ledger", SpnId2);
            /*isBlocked("INVENTORY3", CheckBox14);
            isBlocked("INVENTORY4", CheckBox15);
            isBlocked("INVENTORY5", CheckBox135);*/
        }

        if (IsBlocked("Production", Production) != "1")
        {
            IsBlocked("Production", sub16);
            IsBlocked("Reports", sub16a);
        }
        if (IsBlocked("Accounts", Accounts) != "1")
        {
            IsBlocked("Setups", sub17);
            IsBlocked("Voucher", sub18);
            IsBlocked("Reports", sub19);
        }
        if (IsBlocked("CRM", CRM) != "1")
        {
            IsBlocked("Document Library", sub23);
            //isBlocked("Voucher", sub18);
            //isBlocked("Reports", sub19);
        }
        if (IsBlocked("Admin", Admin) != "1")
        {
            IsBlocked("Setups", sub29);
            IsBlocked("Notice & Messages", sub30);
            IsBlocked("Users & Security", sub31);
            IsBlocked("Admin Reports", sub32);
            IsBlocked("Maintenance", sub33);
        }

        DataTable dt = SQLQuery.ReturnDataTable(@"SELECT sl, MenuGroup, MenuSubGroup, FormName, PageName, HTMLControlID, IsBlocked, EntryBy, EntryDate
                       FROM MenuStructure WHERE sl IN (Select MenuItemID FROM [FormAccessSecurity] where  UserID='" + Page.User.Identity.Name + "')");

        foreach (DataRow dr in dt.Rows)
        {
            string toBlock = dr["HTMLControlID"].ToString();
            HtmlGenericControl ctrl = FindControl(toBlock) as HtmlGenericControl;

            if (ctrl != null)
            {
                ctrl.Visible = false;
            }
        }
    }


}
