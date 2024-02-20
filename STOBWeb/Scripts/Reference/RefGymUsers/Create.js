function ADPLookup()
{
    var ADPEmployeeId = $("#ADPEmployeeId").val();
    $.post(url, { emp: ADPEmployeeId }, function (data) {
        if (data.ADPUser == null) {
            alert("Invalid Employee ID entered. Please enter a valid Employee ID and try again.")
        }
        else {
            $("#FirstName").val(data.ADPUser.FirstName[0].toUpperCase() + data.ADPUser.FirstName.substr(1).toLowerCase());
         
            $("#LastName").val(data.ADPUser.LastName[0].toUpperCase() + data.ADPUser.LastName.substr(1).toLowerCase());
            $("#createbtn").removeAttr('disabled');
            
        }
        //$(document).ready(function () {
        //    $('#mytable').DataTable();
        //});

    });
}