<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="Amazoom.Inventory" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="text-center"><%: Title %></h1>
    <div class="upload-section" style="padding:20px" >
        <h2>Input new inventory</h2>
        <table style="width: 980px">
            <tr>
                <td>
                    <label for="itemID">Item ID:</label><br>
                </td>
                <td>
                    <label for="itemName">Item Name:</label><br>
                </td>
                <td>
                    <label for="itemWeight">Item Weight (kg):</label><br>
                </td>
                <td>
                    <label for="itemVolume">Item Volume (cc):</label><br>
                </td>
                <td style="width: 128px">
                    <label for="itemCount">Item count:</label><br>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="number" id="itemID" name="itemID"><br>
                </td>
                <td>
                    <input type="text" id="itemName" name="itemName">
                </td>
                <td>
                    <input type="number" id="itemWeight" name="itemWeight">
                </td>
                <td>
                    <input type="number" id="itemVolume" name="itemVolume">
                </td>
                <td style="width: 128px">
                    <input type="number" id="itemCount" name="itemCount">
                </td>
            </tr>
        </table>

        <form>
            <asp:Button ID="myButtonAddInventory" runat="server" class="myButtonAddInventory" Text="Add inventory" OnClick="BtnAddInvFromUserInput"/>
        </form>

        

        <h2>Remove inventory</h2>
        <p>
            <table ID="Table1">
                <tr>
                    <td>
                        <label for="itemID">Item ID:</label><br>
                    </td>
                    <td style="width: 128px">
                        <label for="itemCount">Item count:</label><br>
                    </td>
                </tr>
                <tr>
                    <td>
                        <input type="number" id="itemID" name="itemID"><br>
                    </td>
                    <td style="width: 128px">
                        <input type="number" id="itemCount" name="itemCount">
                    </td>
                </tr>
            </table>
            <%--<input id="removeInvetory" type="submit" value="Remove inventory" class="myButtonRemoveInventory" /></p>--%>
            <asp:Button ID="removeInvetory" runat="server" CssClass="myButtonRemoveInventory" Text="Remove Inventory"/>
        <p>
            &nbsp;</p>
        <h3>
            Upload a csv file of inventory from your computer.</h3>
        <p>
            <em>First 4 columns should be <strong>Name, Weight, Volume, Count</strong></em></p>
        <p>
            <asp:FileUpload ID="FileUploadcsvinventory" runat="server" />
        </p>
        <p>
            <asp:Button ID="csvUploadFile" runat="server" Text="Add inventory" CssClass="myButtonAddInventory" OnClick="BtnSaveCSVFile"/>

        </p>
            <asp:Label ID="FileUploadMessage" runat="server" CssClass="alert-info"/>

    </div>
    <div style="padding:20px">
        <h2><strong>Current inventory</strong></h2>
        <%--<table border="1" style="height:150px; width:997px">
            <tbody>
                <tr>
                    <td class="text-center" style="font-weight: bold; width: 227px;"> Item ID</td>
                    <td class="text-center" style="font-weight: bold; width: 203px;"> Name</td>
                    <td class="text-center" style="width: 208px; font-weight: bold"> Weight (kg)</td>
                    <td class="text-center" style="width: 188px" > <strong>Volume (cc)</strong></td>
                    <td class="text-center" > <strong>Count</strong></td>
                </tr>
                <tr>
                    <td class="text-center" style="width: 227px"> Column 1</td>
                    <td class="text-center" style="width: 203px"> Column 2</td>
                    <td class="text-center" style="width: 208px"> Column 3</td>
                    <td class="text-center" style="width: 188px"> Column 4</td>
                    <td class="text-center"> Column 5</td>
                </tr>
                <tr>
                    <td class="text-center" style="width: 227px"> Column 1</td>
                    <td class="text-center" style="width: 203px"> Column 2</td>
                    <td class="text-center" style="width: 208px"> Column 3</td>
                    <td class="text-center" style="width: 188px"> Column 4</td>
                    <td class="text-center"> Column 5</td>
                </tr>
            </tbody>
        </table>--%>
        <br />
        <asp:Table ID="InventoryTable" 
            runat="server" 
            Width="995px" 
            GridLines="Both"
            Font-Names="Verdana" 
            Font-Size="12pt" 
            CellPadding="15" 
            CellSpacing="0"
            Cssclass="text-center"
            >
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Cssclass="text-center" style="font-weight: bold; width: 227px;">Item ID</asp:TableHeaderCell>
                <asp:TableHeaderCell Cssclass="text-center" style="font-weight: bold; width: 227px;">Name</asp:TableHeaderCell>
                <asp:TableHeaderCell Cssclass="text-center" style="font-weight: bold; width: 227px;">Weight (kg)</asp:TableHeaderCell>
                <asp:TableHeaderCell Cssclass="text-center" style="font-weight: bold; width: 227px;">Volume (cc)</asp:TableHeaderCell>
                <asp:TableHeaderCell Cssclass="text-center" style="font-weight: bold; width: 227px;">Count</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    
</asp:Content>

