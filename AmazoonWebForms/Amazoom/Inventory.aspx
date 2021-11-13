<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="Amazoom.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="text-center"><%: Title %><strong>Inventory</strong></h1>
    <div style="padding:20px">
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
          <input type="submit" value="Add to inventory"/>
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
            <input id="removeInvetory" type="submit" value="Remove from inventory" /></p>

    </div>
    <div>
        <h2><strong>Current inventory</strong></h2>
        <table border="1" style="height:150px; width:929px">
            <tbody>
                <tr>
                    <td class="text-center" style="font-weight: bold; width: 257px;"> Item ID</td>
                    <td class="text-center" style="font-weight: bold; width: 218px;"> Name</td>
                    <td class="text-center" style="width: 238px; font-weight: bold"> Weight (kg)</td>
                    <td class="text-center" > <strong>Volume (cc)</strong></td>
                </tr>
                <tr>
                    <td class="text-center" style="width: 257px"> Column 1</td>
                    <td class="text-center" style="width: 218px"> Column 2</td>
                    <td class="text-center" style="width: 238px"> Column 3</td>
                    <td class="text-center"> Column 1</td>
                </tr>
                <tr>
                    <td class="text-center" style="width: 257px"> Column 1</td>
                    <td class="text-center" style="width: 218px"> Column 2</td>
                    <td class="text-center" style="width: 238px"> Column 3</td>
                    <td class="text-center"> Column 1</td>
                </tr>
            </tbody>
        </table>
    </div>
    
</asp:Content>
