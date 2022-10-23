using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class app_Grinding : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        ////btnSave.Attributes.Add("onclick", " this.disabled = true; " + ClientScript.GetPostBackEventReference(btnSave, null) + ";");        

        if (!IsPostBack)
        {
            txtDate.Text = DateTime.Today.Date.ToShortDateString();
            string lName = Page.User.Identity.Name.ToString();
            lblProject.Text = RunQuery.SQLQuery.ReturnString("SELECT ProjectID FROM Logins WHERE LoginUserName ='" + lName + "'");

            string gQuery = "SELECT CategoryID, CategoryName FROM [ItemSubGroup] where CategoryID='9' AND ProjectID='" + lblProject.Text + "' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddSubGrp, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='9' AND ProjectID='" + lblProject.Text + "' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGradeRaw, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND CategoryName<>'Body' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");

            gQuery = "SELECT GradeID, GradeName FROM [ItemGrade] where CategoryID='17' AND ProjectID='" + lblProject.Text + "' AND GradeName<>'Body' ORDER BY [GradeName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddGrade2, "GradeID", "GradeName");

            gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGrade2.SelectedValue + "' AND CategoryName<>'Body' ORDER BY [CategoryName]";
            RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory2, "CategoryID", "CategoryName");

            GetProductList();
        }
    }
    //Code for Clearing the form
    public static void ClearControls(Control Parent)
    {
        if (Parent is TextBox)
        { (Parent as TextBox).Text = string.Empty; }
        else
        {
            foreach (Control c in Parent.Controls)
                ClearControls(c);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string mpo = Page.User.Identity.Name.ToString();
            mpo = mpo.Trim();

            //if (Convert.ToDecimal(txtPaid.Text) > 0)
            //{
            SavePayment();
            GridView1.DataBind();
            //    MessageBox("Payment Entry Saves Successfully.");
            //}
            //else
            //{
            //    lblMsg.Text = "Please Check Payment Amount Properly";
            //    lblMsg.ForeColor = System.Drawing.ColorTranslator.FromHtml("#6e000a");
            //}


            lblMsg.Attributes.Add("class", "xerp_error");
            lblMsg.Text = "Error: Input weight & output weight must have to be equel!";
        }
        catch (Exception ex)
        {
            lblMsg.Attributes.Add("class", "xerp_warning");
            lblMsg.Text = "Error: " + ex.ToString();
        }
    }

    private void SavePayment()
    {
        if (txtFinalProd.Text == "")
        {
            txtFinalProd.Text = "0";
        }
        string lName = Page.User.Identity.Name.ToString();
        string prdId = RunQuery.SQLQuery.ReturnString("Select 'PRD-'+ CONVERT(varchar, (ISNULL (max(pid),0)+1001 )) from Production");

        SqlCommand cmd2 = new SqlCommand("INSERT INTO Production (ProductionID, Date, SectionID, SectionName, MachineNo, LineNumber, SizeID, Purpose, ItemID, ItemName, Color, Process, CustomerID, CustomerName, Operation, OperatorID, Hour, Production, Rejection, TimeWaste, ReasonWaist, Shift, FinalProduction, EntryBy) VALUES (@ProductionID, @Date, @SectionID, @SectionName, @MachineNo, @LineNumber, @SizeID, @Purpose, @ItemID, @ItemName, @Color, @Process, @CustomerID, @CustomerName, @Operation, @OperatorID, @Hour, @Production, @Rejection, @TimeWaste, @ReasonWaist, @Shift, @FinalProduction, @EntryBy)", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));

        cmd2.Parameters.AddWithValue("@ProductionID", prdId);
        cmd2.Parameters.AddWithValue("@Date", Convert.ToDateTime(txtDate.Text).ToString("yyyy-MM-dd"));
        cmd2.Parameters.AddWithValue("@SectionID", "2");
        cmd2.Parameters.AddWithValue("@SectionName", "Shearing");

        cmd2.Parameters.AddWithValue("@MachineNo", ddMachine.SelectedValue);
        cmd2.Parameters.AddWithValue("@LineNumber", ddLine.SelectedValue);
        cmd2.Parameters.AddWithValue("@SizeID", ddSize.SelectedValue);
        cmd2.Parameters.AddWithValue("@Purpose", txtPurpose.Text);
        cmd2.Parameters.AddWithValue("@ItemID", ddProduct.SelectedValue);
        cmd2.Parameters.AddWithValue("@ItemName", ddProduct.SelectedItem.Text);

        cmd2.Parameters.AddWithValue("@Color", ""); //ddColor.SelectedValue);
        cmd2.Parameters.AddWithValue("@Process", ""); //ddProcess.SelectedValue);
        cmd2.Parameters.AddWithValue("@CustomerID", ddCustomer.SelectedValue);
        cmd2.Parameters.AddWithValue("@CustomerName", ddCustomer.SelectedItem.Text);
        cmd2.Parameters.AddWithValue("@Operation", "");// ddOperation.SelectedValue);
        cmd2.Parameters.AddWithValue("@OperatorID", "");// ddOperator.SelectedValue);
        cmd2.Parameters.AddWithValue("@Hour", txtHour.Text);

        cmd2.Parameters.AddWithValue("@Production", txtproduced.Text);
        cmd2.Parameters.AddWithValue("@Rejection", "0");
        cmd2.Parameters.AddWithValue("@TimeWaste", Convert.ToInt32(txtTimeWaist.Text));
        cmd2.Parameters.AddWithValue("@ReasonWaist", txtReason.Text);
        cmd2.Parameters.AddWithValue("@Shift", ddShift.SelectedValue);
        cmd2.Parameters.AddWithValue("@FinalProduction", txtFinalProd.Text);
        cmd2.Parameters.AddWithValue("@EntryBy", lName);

        cmd2.Connection.Open();
        cmd2.ExecuteNonQuery();
        cmd2.Connection.Close();

        int finalProd = Convert.ToInt32(txtFinalProd.Text);
        if (finalProd > 0)
        {
            //Accounting.VoucherEntry.ProductionStockEntry(prdId, "Processed Stock-in from Power-Press", ddSize.SelectedValue, ddCustomer.SelectedValue, ddProduct.SelectedValue, ddProduct.SelectedItem.Text, "2", "3", txtproduced.Text, "0", txtRemark.Text, "Production", "Processed", "", lName);
            //Accounting.VoucherEntry.ProductionStockEntry(prdId, "Wastage Stock-in from Power-Press", ddSize.SelectedValue, ddCustomer.SelectedValue, ddProduct.SelectedValue, ddProduct.SelectedItem.Text, "2", "3", txtRejected.Text, "0", txtRemark.Text, "Production", "Wastage", "", lName);
        }
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddMode.SelectedValue == "Cheque")
        //{
        //    chqpanel.Visible = true;
        //}
        //else
        //{
        //    chqpanel.Visible = false;
        //    txtPaid.Focus();
        //}
    }


    protected void ddProduct_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }
    protected void ddSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ddOperator.DataBind();
    }

    protected void ddGradeRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT CategoryID, CategoryName FROM [Categories] where GradeID='" + ddGradeRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' AND CategoryName<>'Body' ORDER BY [CategoryName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategoryRaw, "CategoryID", "CategoryName");
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddCategory2, "CategoryID", "CategoryName");

        GetProductList();
    }
    protected void ddCategoryRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddCategory2.SelectedValue = ddCategoryRaw.SelectedValue;
        GetProductList();

    }
    protected void ddItemNameRaw_SelectedIndexChanged(object sender, EventArgs e)
    {
        StockDetails();
    }

    private void GetProductList()
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategoryRaw.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddItemNameRaw, "ProductID", "ItemName");

        gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory2.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddProduct, "ProductID", "ItemName");
        StockDetails();
    }

    private void StockDetails()
    {
        decimal availableWeight = Convert.ToDecimal(RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where  ProductID='" + ddItemNameRaw.SelectedValue + "'")) / 1000M;
        ltrLastInfo.Text = "Stock Qty.: " + RunQuery.SQLQuery.ReturnString("SELECT ISNULL(sum(InQuantity)-Sum(OutQuantity),0) FROM Stock where  ProductID='" + ddItemNameRaw.SelectedValue + "'") + " PCS, " + availableWeight + "KG";
    }
    protected void ddCategory2_SelectedIndexChanged(object sender, EventArgs e)
    {
        string gQuery = "SELECT ProductID, ItemName FROM [Products] where CategoryID='" + ddCategory2.SelectedValue + "' AND ProjectID='" + lblProject.Text + "' ORDER BY [ItemName]";
        RunQuery.SQLQuery.PopulateDropDown(gQuery, ddProduct, "ProductID", "ItemName");
        StockDetails();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            /*
            if (txtName.Text != "" && ddGrade.SelectedValue != "" && ddSubGrp.SelectedValue != "" && ddcategory.SelectedValue != "")
            {
                if (btnSave.Text == "Save")
                {
                    SqlCommand cmde = new SqlCommand("SELECT ItemName FROM Products WHERE CategoryID ='" + ddcategory.SelectedValue + "' AND  ItemName ='" + txtName.Text + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
                    cmde.Connection.Open();
                    string 
                    = Convert.ToString(cmde.ExecuteScalar());
                    cmde.Connection.Close();

                    if (isExist == "")
                    {
                        ExecuteInsert();
                        EditField.Attributes.Add("class", "form-group hidden");
                        txtName.Text = "";
                        lblMsg.Attributes.Add("class", "xerp_success");
                        lblMsg.Text = "New product added successfully";
                    }
                    else
                    {
                        lblMsg.Attributes.Add("class", "xerp_error");
                        lblMsg.Text = "Error: Item already exist!";
                    }
                }
                else
                {
                    ExecuteUpdate();
                    ClearControls(Form);
                    btnSave.Text = "Save";
                    EditField.Attributes.Add("class", "form-group hidden");

                    lblMsg.Attributes.Add("class", "xerp_success");
                    lblMsg.Text = "Item updated successfully";
                }
            }
            else
            {
                lblMsg.Attributes.Add("class", "xerp_error");
                lblMsg.Text = "Please fillup all mendatory fields...";
            }
             */

            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Error: Input weight & output weight must have to be equel!";
        }
        catch (Exception ex)
        {
            lblMsg2.Attributes.Add("class", "xerp_error");
            lblMsg2.Text = "Error: " + ex.Message.ToString();
        }
        finally
        {
            //GridView2.DataBind();

        }

    }
    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddBrand.DataBind();
    }
}
