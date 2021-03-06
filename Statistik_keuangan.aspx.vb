﻿Imports System.Data
Imports System.Data.SqlClient

Partial Class Statistik_keuangan
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            getCustRecords("", "")
            Dim i As Integer
            For i = 0 To CustGrid.HeaderRow.Cells.Count - 1
                Dim Link As LinkButton = CustGrid.HeaderRow.Cells(i).Controls(0)
                ddlSearchBy.Items.Add(Link.Text)
            Next
            searchValue.Enabled = False
            BindGrid2()
        End If
    End Sub
    Protected Sub CustGrid_OnSorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)
        'Retrieve the table from the session object.
        Dim dt = TryCast(Session("data"), DataTable)
        If dt IsNot Nothing Then
            'Sort the data.
            dt.DefaultView.Sort = e.SortExpression & " " & GetSortDirection(e.SortExpression)
            CustGrid.DataSource = Session("data")
            CustGrid.DataBind()
        End If
    End Sub

    Private Function GetSortDirection(ByVal column As String) As String
        ' By default, set the sort direction to ascending.
        Dim sortDirection = "ASC"
        ' Retrieve the last column that was sorted.
        Dim sortExpression = TryCast(ViewState("SortExpression"), String)
        If sortExpression IsNot Nothing Then
            ' Check if the same column is being sorted.
            ' Otherwise, the default value can be returned.
            If sortExpression = column Then
                Dim lastDirection = TryCast(ViewState("SortDirection"), String)
                If lastDirection IsNot Nothing _
          AndAlso lastDirection = "ASC" Then
                    sortDirection = "DESC"
                End If
            End If
        End If
        ' Save new values in ViewState.
        ViewState("SortDirection") = sortDirection
        ViewState("SortExpression") = column
        Return sortDirection
    End Function

    Protected Sub CustGrid_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles CustGrid.PageIndexChanging
        CustGrid.PageIndex = e.NewPageIndex
        CustGrid.DataSource = Session("data")
        CustGrid.DataBind()
    End Sub
#Region "searching"
    Protected Sub ddlSearchBy_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlSearchBy.SelectedIndexChanged
        If ddlSearchBy.SelectedItem.Text = "All" Then
            searchValue.Text = String.Empty
            searchValue.Enabled = False
        Else
            searchValue.Enabled = True
            searchValue.Text = String.Empty
            searchValue.Focus()
        End If
    End Sub

    Protected Sub searchBtn_Click(sender As Object, e As System.EventArgs) Handles searchBtn.Click
        getCustRecords(ddlSearchBy.SelectedItem.Text, searchValue.Text.Trim())
    End Sub

    Private Sub getCustRecords(searchBy As String, searchVal As String)
        Dim constring As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Dim constr As New SqlConnection(constring)
        Dim dt As New DataTable()
        Dim cmd As New SqlCommand()
        Dim sda As New SqlDataAdapter()
        cmd = New SqlCommand("Transaksi_SP", constr)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.AddWithValue("@Action", "SELECT_TRANSAKSI")
        cmd.Parameters.AddWithValue("@id_transaksi", "")
        cmd.Parameters.AddWithValue("@nama_transaksi", "")
        cmd.Parameters.AddWithValue("@jenis_transaksi", "")
        cmd.Parameters.AddWithValue("@kode_transaksi", "")
        cmd.Parameters.AddWithValue("@tanggal_transaksi", "")
        cmd.Parameters.AddWithValue("@debit", "")
        cmd.Parameters.AddWithValue("@kredit", "")
        cmd.Parameters.AddWithValue("@saldo", "")
        cmd.Parameters.AddWithValue("@waktu_input_data", "")
        sda.SelectCommand = cmd
        sda.Fill(dt)
        If dt.Rows.Count > 0 Then
            Session("data") = dt
            CustGrid.DataSource = dt
            CustGrid.DataBind()
        Else
            CustGrid.DataSource = dt
            CustGrid.DataBind()
        End If
    End Sub
#End Region

    Protected Sub OnRowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(CustGrid, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("style") = "cursor:pointer"
        End If
    End Sub
    Protected Sub OnSelectedIndexChanged(sender As Object, e As EventArgs)
        Dim Index As Integer = CustGrid.SelectedRow.RowIndex
        Session("select") = CustGrid.SelectedRow.Cells(0).Text.ToString
    End Sub

    Protected Sub clear_Click(sender As Object, e As EventArgs) Handles clear.Click 'clear the search textbox and reload the page
        searchValue.Text = String.Empty
        Response.Redirect(HttpContext.Current.Request.Url.ToString(), True)
    End Sub
    Private Sub BindGrid2()
        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
        Using con As New SqlConnection(constr)
            Using cmd As New SqlCommand("Transaksi_SP")
                cmd.Parameters.AddWithValue("@Action", "SELECT_SALDO")
                cmd.Parameters.AddWithValue("@id_transaksi", "")
                cmd.Parameters.AddWithValue("@nama_transaksi", "")
                cmd.Parameters.AddWithValue("@jenis_transaksi", "")
                cmd.Parameters.AddWithValue("@kode_transaksi", "")
                cmd.Parameters.AddWithValue("@tanggal_transaksi", "")
                cmd.Parameters.AddWithValue("@debit", "")
                cmd.Parameters.AddWithValue("@kredit", "")
                cmd.Parameters.AddWithValue("@saldo", "")
                cmd.Parameters.AddWithValue("@waktu_input_data", "")
                Using sda As New SqlDataAdapter()
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Connection = con
                    sda.SelectCommand = cmd
                    Using dt As New DataTable()
                        sda.Fill(dt)
                        saldo.DataSource = dt
                        saldo.DataBind()
                    End Using
                End Using
            End Using
        End Using
    End Sub
End Class



'Imports System.Data
'Imports System.Configuration
'Imports System.Data.SqlClient
'Partial Class Statistik_keuangan
'    Inherits System.Web.UI.Page

'    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
'        If Not Me.IsPostBack Then
'            Me.BindGrid()
'            Me.BindGrid2()
'        End If
'    End Sub

'    Private Sub BindGrid()
'        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
'        Using con As New SqlConnection(constr)
'            Using cmd As New SqlCommand("Transaksi_SP")
'                cmd.Parameters.AddWithValue("@Action", "SELECT_TRANSAKSI")
'                cmd.Parameters.AddWithValue("@id_transaksi", "")
'                cmd.Parameters.AddWithValue("@nama_transaksi", "")
'                cmd.Parameters.AddWithValue("@jenis_transaksi", "")
'                cmd.Parameters.AddWithValue("@kode_transaksi", "")
'                cmd.Parameters.AddWithValue("@tanggal_transaksi", "")
'                cmd.Parameters.AddWithValue("@debit", "")
'                cmd.Parameters.AddWithValue("@kredit", "")
'                cmd.Parameters.AddWithValue("@saldo", "")
'                cmd.Parameters.AddWithValue("@waktu_input_data", "")
'                Using sda As New SqlDataAdapter()
'                    cmd.CommandType = CommandType.StoredProcedure
'                    cmd.Connection = con
'                    sda.SelectCommand = cmd
'                    Using dt As New DataTable()
'                        sda.Fill(dt)
'                        transaksi.DataSource = dt
'                        transaksi.DataBind()
'                    End Using
'                End Using
'            End Using
'        End Using
'    End Sub
'    Private Sub BindGrid2()
'        Dim constr As String = ConfigurationManager.ConnectionStrings("constr").ConnectionString
'        Using con As New SqlConnection(constr)
'            Using cmd As New SqlCommand("Transaksi_SP")
'                cmd.Parameters.AddWithValue("@Action", "SELECT_SALDO")
'                cmd.Parameters.AddWithValue("@id_transaksi", "")
'                cmd.Parameters.AddWithValue("@nama_transaksi", "")
'                cmd.Parameters.AddWithValue("@jenis_transaksi", "")
'                cmd.Parameters.AddWithValue("@kode_transaksi", "")
'                cmd.Parameters.AddWithValue("@tanggal_transaksi", "")
'                cmd.Parameters.AddWithValue("@debit", "")
'                cmd.Parameters.AddWithValue("@kredit", "")
'                cmd.Parameters.AddWithValue("@saldo", "")
'                cmd.Parameters.AddWithValue("@waktu_input_data", "")
'                Using sda As New SqlDataAdapter()
'                    cmd.CommandType = CommandType.StoredProcedure
'                    cmd.Connection = con
'                    sda.SelectCommand = cmd
'                    Using dt As New DataTable()
'                        sda.Fill(dt)
'                        saldo.DataSource = dt
'                        saldo.DataBind()
'                    End Using
'                End Using
'            End Using
'        End Using
'    End Sub
'End Class
