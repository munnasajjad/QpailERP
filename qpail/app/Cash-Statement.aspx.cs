using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class AdminCentral_Cash_Statement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //txtDateFrom.Text = DateTime.Now.ToShortDateString();
            //txtDateTo.Text = DateTime.Now.ToShortDateString();
            LoadGridData();
        }
        //Get Branch/Centre Name
        string lName = Page.User.Identity.Name.ToString();
        SqlCommand cmdxx = new SqlCommand("Select BranchName from Users where Username='" + lName + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        cmdxx.Connection.Open();
        //Label1.Text = Convert.ToString(cmdxx.ExecuteScalar());
        cmdxx.Connection.Close();
        cmdxx.Connection.Dispose();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        LoadGridData();
        GridView1.DataBind();
    }

    private void LoadGridData()
    {

        SqlDataAdapter da;
        SqlDataReader dr;
        DataSet ds;
        int recordcount = 0;
        int ic = 0;
        SqlCommand cmd = new SqlCommand("Select * from Branches", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
        da = new SqlDataAdapter(cmd);
        ds = new DataSet("Board");

        cmd.Connection.Open();
        da.Fill(ds, "Board");

        dr = cmd.ExecuteReader();
        recordcount = ds.Tables[0].Rows.Count;

        DataTable dt1 = new DataTable();
        DataRow dr1 = null;
        dt1.Columns.Add(new DataColumn("Branch_Name", typeof(string)));
        dt1.Columns.Add(new DataColumn("District", typeof(string)));
        dt1.Columns.Add(new DataColumn("Credit_Limit", typeof(string)));
        dt1.Columns.Add(new DataColumn("Current_Limit", typeof(string)));
        dt1.Columns.Add(new DataColumn("Current_Balance", typeof(string)));
        
        decimal currLimit = 0; decimal currBal = 0;
        string branch; string district; decimal creditLimit;

        do
        {
            branch = ds.Tables[0].Rows[ic]["BranchName"].ToString();
            district = ds.Tables[0].Rows[ic]["District"].ToString();
            creditLimit = Convert.ToDecimal(ds.Tables[0].Rows[ic]["CreditLimit"].ToString());

            //Get current Balance
            SqlCommand cmdb = new SqlCommand("SELECT isnull(SUM(Dr),0) - isnull(SUM(Cr),0) FROM Transactions WHERE HeadName ='" + branch + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdb.Connection.Open();
            currBal = Convert.ToDecimal(cmdb.ExecuteScalar());
            cmdb.Connection.Close();
            cmdb.Connection.Dispose();

            currLimit = creditLimit - currBal;

            dr1 = dt1.NewRow();
            dr1["Branch_Name"] = branch;
            dr1["District"] = district;
            dr1["Credit_Limit"] = creditLimit;
            dr1["Current_Limit"] = currLimit;
            dr1["Current_Balance"] = currBal;
            dt1.Rows.Add(dr1);

            /*
            //get details of upper leader as a member
            SqlCommand cmd2y = new SqlCommand("Select  BoardEntryID, UpperMemberID, LeftHandID, RightHandID, UpperEntryID from Board where MemberID=@MemberID", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2y.Parameters.Add("@MemberID", SqlDbType.VarChar).Value = LeaderId;

            cmd2y.Connection.Open();
            SqlDataReader dr1y = cmd2y.ExecuteReader();

            if (dr1y.Read())
            {
                left = dr1y[2].ToString();
                right = dr1y[3].ToString();
            }
            cmd2y.Connection.Close();

            // Total Given Matching Bonus
            SqlCommand cmdf = new SqlCommand("SELECT ISNULL(SUM([CommissionAmt]),0) FROM [Commission] WHERE [MemberID] ='" + LeaderId + "' AND (Description = 'Team Growth Point')", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmdf.Connection.Open();
            givenbonus = Convert.ToDecimal(cmdf.ExecuteScalar());
            cmdf.Connection.Close();
            cmdf.Connection.Dispose();

            // bord entry id of left member
            SqlCommand cmd2v1f = new SqlCommand("SELECT BoardEntryID FROM Board where MemberID='" + left + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2v1f.Connection.Open();
            boardIdl = Convert.ToInt32(cmd2v1f.ExecuteScalar());
            cmd2v1f.Connection.Close();
            cmd2v1f.Connection.Dispose();
            // bord entry id of right member
            SqlCommand cmd2v1f1 = new SqlCommand("SELECT BoardEntryID FROM Board where MemberID='" + right + "'", new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString));
            cmd2v1f1.Connection.Open();
            boardIdr = Convert.ToInt32(cmd2v1f1.ExecuteScalar());
            cmd2v1f1.Connection.Close();
            cmd2v1f1.Connection.Dispose();

            //count left side
            DataSet ds20 = RunQuery("Select BoardEntryID,MemberID from Board where UpperEntryID='" + boardIdl + "'");
            for (int i = 0; i < ds20.Tables[0].Rows.Count; i++)
            {
                TreeNode root = new TreeNode(ds20.Tables[0].Rows[i][1].ToString(), ds20.Tables[0].Rows[i][0].ToString());
                root.SelectAction = TreeNodeSelectAction.Expand;
                CreateNode(root);
                TreeView1.Nodes.Add(root);
                leftCount++;
            }
            TreeView1.Nodes.Clear();
            //count right side
            DataSet ds21 = RunQuery("Select BoardEntryID,MemberID from Board where UpperEntryID='" + boardIdr + "'");
            for (int i = 0; i < ds21.Tables[0].Rows.Count; i++)
            {
                TreeNode root = new TreeNode(ds21.Tables[0].Rows[i][1].ToString(), ds21.Tables[0].Rows[i][0].ToString());
                root.SelectAction = TreeNodeSelectAction.Expand;
                CreateNodeR(root);
                TreeView1.Nodes.Add(root);
                rightCount++;
            }
            if (left != "")
            {
                leftCount++;
            }
            if (right != "")
            {
                rightCount++;
            }
            if (leftCount >= rightCount) { bonus = rightCount * 4.5M; }
            else { bonus = leftCount * 4.5M; }
             */

            ic++;

        } while (ic < recordcount);
        GridView1.DataSource = dt1;
        GridView1.DataBind();


    }

}
