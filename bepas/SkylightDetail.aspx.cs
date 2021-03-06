﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace bepas
{
    public partial class SkylightDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                LoadDropdownItems();
                LoadSiteList();

            } //if

        } //Page_Load()

        private void LoadDropdownItems()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["bepas"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = "spLoadDropdownItems";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Connection = connection;

                connection.Open();

                using (DataSet dataSet = new DataSet())
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    command.Parameters.AddWithValue("@uid", 14);
                    adapter.Fill(dataSet);
                    ddlSkylightOrientation.DataSource = dataSet;
                    ddlSkylightOrientation.DataTextField = "value";
                    ddlSkylightOrientation.DataValueField = "uid";
                    ddlSkylightOrientation.DataBind();

                    command.Parameters["@uid"].Value = 32;
                    dataSet.Clear();
                    adapter.Fill(dataSet);
                    ddlSkylightType.DataSource = dataSet;
                    ddlSkylightType.DataTextField = "value";
                    ddlSkylightType.DataValueField = "uid";
                    ddlSkylightType.DataBind();

                    command.Parameters["@uid"].Value = 33;
                    dataSet.Clear();
                    adapter.Fill(dataSet);
                    ddlGlazing.DataSource = dataSet;
                    ddlGlazing.DataTextField = "value";
                    ddlGlazing.DataValueField = "uid";
                    ddlGlazing.DataBind();

                    command.Parameters["@uid"].Value = 34;
                    dataSet.Clear();
                    adapter.Fill(dataSet);
                    ddlCoating.DataSource = dataSet;
                    ddlCoating.DataTextField = "value";
                    ddlCoating.DataValueField = "uid";
                    ddlCoating.DataBind();

                    command.Parameters["@uid"].Value = 35;
                    dataSet.Clear();
                    adapter.Fill(dataSet);
                    ddlExteriorShading.DataSource = dataSet;
                    ddlExteriorShading.DataTextField = "value";
                    ddlExteriorShading.DataValueField = "uid";
                    ddlExteriorShading.DataBind();
                } //using SqlDataAdapter
            } //using SqlCommand


            ddlSkylightOrientation.Items.Insert(0, new ListItem("Please Select", "-1"));
            ddlSkylightType.Items.Insert(0, new ListItem("Please Select", "-1"));
            ddlGlazing.Items.Insert(0, new ListItem("Please Select", "-1"));
            ddlCoating.Items.Insert(0, new ListItem("Please Select", "-1"));
            ddlExteriorShading.Items.Insert(0, new ListItem("Please Select", "-1"));

        } //LoadDropdownItems()

        private void LoadSiteList()
        {
            DataSet dataSet = GetDataUsingSp("spLoadSites", null, null);
            gvSiteList.DataSource = dataSet;
            gvSiteList.DataBind();
            gvSiteList.HeaderRow.TableSection = TableRowSection.TableHeader;
        } //LoadSiteList()

        private void LoadBuildingList(int siteUid)
        {
            DataSet dataSet = GetDataUsingSp("spLoadBuildings", "@siteUid", siteUid);
            gvBuildingList.DataSource = dataSet;
            gvBuildingList.DataBind();
            gvBuildingList.HeaderRow.TableSection = TableRowSection.TableHeader;
        } //LoadBuildingList()

        private void LoadRoomList(int buildingUid)
        {
            DataSet dataSet = GetDataUsingSp("spLoadRoomList", "@buildingUid", buildingUid);
            gvRoomList.DataSource = dataSet;
            gvRoomList.DataBind();
            gvRoomList.HeaderRow.TableSection = TableRowSection.TableHeader;
        } //LoadRoomList()

        private void LoadSkylightList(int roomUid)
        {
            DataSet dataSet = GetDataUsingSp("spLoadSkylightList", "@roomUid", roomUid);
            gvSkylightList.DataSource = dataSet;
            gvSkylightList.DataBind();
            gvSkylightList.HeaderRow.TableSection = TableRowSection.TableHeader;
        } //LoadSkylightList()

        protected void gvSiteListOnRowCommandSelect(object sender, GridViewCommandEventArgs e)
        {
            SuccessPanel.Visible = false;
            buildingId.Text = String.Empty;
            buildingName.Text = String.Empty;
            roomId.Text = String.Empty;
            roomName.Text = String.Empty;
            skylightId.Text = String.Empty;
            skylightName.Text = String.Empty;
            ClearInputFields();

            string[] argument = new string[3];
            argument = e.CommandArgument.ToString().Split(';');

            string siteUidLocal = argument[0];
            string siteIdByUserLocal = argument[1];
            string siteNameLocal = argument[2];

            siteId.Text = siteIdByUserLocal;
            siteName.Text = siteNameLocal;
            LoadBuildingList(Convert.ToInt32(siteUidLocal));
        }

        protected void gvBuildingListOnRowCommandSelect(object sender, GridViewCommandEventArgs e)
        {
            SuccessPanel.Visible = false;
            roomId.Text = String.Empty;
            roomName.Text = String.Empty;
            skylightId.Text = String.Empty;
            skylightName.Text = String.Empty;
            ClearInputFields();

            string[] argument = new string[3];
            argument = e.CommandArgument.ToString().Split(';');

            string buildingUidLocal = argument[0];
            string buildingIdByUserLocal = argument[1];
            string buildingNameLocal = argument[2];

            buildingId.Text = buildingIdByUserLocal;
            buildingName.Text = buildingNameLocal;
            LoadRoomList(Convert.ToInt32(buildingUidLocal));
        }

        protected void gvRoomListOnRowCommandSelect(object sender, GridViewCommandEventArgs e)
        {
            SuccessPanel.Visible = false;
            skylightId.Text = String.Empty;
            skylightName.Text = String.Empty;
            ClearInputFields();

            string[] argument = new string[3];
            argument = e.CommandArgument.ToString().Split(';');
            
            string roomUidLocal = argument[0];
            string roomIdByUserLocal = argument[1];
            string roomNameLocal = argument[2];

            roomId.Text = roomIdByUserLocal;
            roomName.Text = roomNameLocal;
            LoadSkylightList(Convert.ToInt32(roomUidLocal));
        }

        protected void gvSkylightListOnRowCommandSelect(object sender, GridViewCommandEventArgs e)
        {
            SuccessPanel.Visible = false; 

            string[] argument = new string[3];
            argument = e.CommandArgument.ToString().Split(';');
            
            string skylightUidLocal = argument[0];
            string skylightIdByUserLocal = argument[1];
            string skylightNameLocal = argument[2];

            ViewState["roomUid"] = skylightUidLocal;
            skylightId.Text = skylightIdByUserLocal;
            skylightName.Text = skylightNameLocal;
            LoadInputFields(Convert.ToInt32(skylightUidLocal));
        }

        private void LoadInputFields(int skylightUid)
        {
            DataSet dataSet = GetDataUsingSp("spLoadSkylightDetail", "@skylightUid", skylightUid);

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dataSet.Tables[0].Rows[0];
                ddlSkylightOrientation.SelectedValue = dr["skylightOrientationId"].ToString();
                ddlSkylightType.SelectedValue = dr["skylightTypeId"].ToString();
                numberOfSkylights.Text = dr["numberOfSkylights"].ToString();
                skylightLength.Text = dr["skylightLength"].ToString();
                skylightWidth.Text = dr["skylightWidth"].ToString();
                ddlGlazing.SelectedValue = dr["glazingId"].ToString();
                ddlCoating.SelectedValue = dr["coatingId"].ToString();
                ddlExteriorShading.SelectedValue = dr["exteriorShadingId"].ToString();
                notes.Value = dr["notes"].ToString();
            }
            else
            {
                ClearInputFields();
            }
        } //LoadInputFields()

        private DataSet GetDataUsingSp(string spName, string spParameterName, object spParameter)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["bepas"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))

            using (SqlCommand command = new SqlCommand())
            {
                command.CommandText = spName;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Connection = connection;

                DataSet dataSet = new DataSet();

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    if (spParameter != null)
                        command.Parameters.AddWithValue(spParameterName, (int)spParameter);
                    connection.Open();
                    adapter.Fill(dataSet);
                } //using SqlDataAdapter
                return dataSet;
            } //using SqlCommand
        } //GetDataUsingSp()

        protected void cancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~");
        }

        protected void addButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid) //checks validation again in case javascript disabled <-- havent tested this yet
            {

                string connectionString = ConfigurationManager.ConnectionStrings["bepas"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))

                using (SqlCommand command = new SqlCommand())
                {
                    int UserUid = 1;
                    command.CommandText = "spInsertSkylight";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@roomUid", Convert.ToInt32(ViewState["roomUid"]));
                    command.Parameters.AddWithValue("@skylightIdByUser", skylightId.Text);
                    command.Parameters.AddWithValue("@skylightName", skylightName.Text);
                    command.Parameters.AddWithValue("@skylightOrientationId", Convert.ToInt32(ddlSkylightOrientation.SelectedValue));
                    command.Parameters.AddWithValue("@skylightOrientationText", ddlSkylightOrientation.SelectedItem.Text);
                    command.Parameters.AddWithValue("@skylightTypeId", Convert.ToInt32(ddlSkylightType.SelectedValue));
                    command.Parameters.AddWithValue("@skylightTypeText", ddlSkylightType.SelectedItem.Text);
                    command.Parameters.AddWithValue("@numberOfskylights", numberOfSkylights.Text);
                    command.Parameters.AddWithValue("@skylightLength", skylightLength.Text);
                    command.Parameters.AddWithValue("@skylightWidth", skylightWidth.Text);
                    command.Parameters.AddWithValue("@glazingId", Convert.ToInt32(ddlGlazing.SelectedValue));
                    command.Parameters.AddWithValue("@glazingText", ddlGlazing.SelectedItem.Text);
                    command.Parameters.AddWithValue("@coatingId", Convert.ToInt32(ddlCoating.SelectedValue));
                    command.Parameters.AddWithValue("@coatingText", ddlCoating.SelectedItem.Text);
                    command.Parameters.AddWithValue("@exteriorShadingId", Convert.ToInt32(ddlExteriorShading.SelectedValue));
                    command.Parameters.AddWithValue("@exteriorShadingText", ddlExteriorShading.SelectedItem.Text);
                    command.Parameters.Add("@skylightPhoto", SqlDbType.VarBinary).Value = DBNull.Value;
                    command.Parameters.AddWithValue("@skylightPhotoFileName", DBNull.Value);
                    command.Parameters.AddWithValue("@notes", notes.InnerText);
                    command.Parameters.AddWithValue("@userId", UserUid);
                    connection.Open();
                    command.ExecuteNonQuery();
                    SuccessPanel.Visible = true;
                }
            } // if(page valid)

        } //addButton_Click()

        private void ClearInputFields()
        {
            ddlSkylightOrientation.SelectedValue = "-1";
            ddlSkylightType.SelectedValue = "-1";
            numberOfSkylights.Text = String.Empty;
            skylightLength.Text = String.Empty;
            skylightWidth.Text = String.Empty;
            ddlGlazing.SelectedValue = "-1";
            ddlCoating.SelectedValue = "-1";
            ddlExteriorShading.SelectedValue = "-1";
            notes.Value = String.Empty;
        }

    } //Webform
} //namespace bepas