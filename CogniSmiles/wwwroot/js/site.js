// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function CopyAddress() {
    var chkVal = $("#chkAddress")[0].checked;
    var al1 = "";
    var al2 = "";
    var al3 = "";
    var city = "";
    var postalCode = "";
    if (chkVal) {
        al1 = $("#DoctorDetails_DeliveryAddress_AddressLine1")[0].value;
        al2 = $("#DoctorDetails_DeliveryAddress_AddressLine2")[0].value;
        al3 = $("#DoctorDetails_DeliveryAddress_AddressLine3")[0].value;
        city = $("#DoctorDetails_DeliveryAddress_City")[0].value;
        postalCode = $("#DoctorDetails_DeliveryAddress_PostalCode")[0].value;
    }
    $("#DoctorDetails_BillingAddress_AddressLine1")[0].value = al1;
    $("#DoctorDetails_BillingAddress_AddressLine2")[0].value = al2;
    $("#DoctorDetails_BillingAddress_AddressLine3")[0].value = al3;
    $("#DoctorDetails_BillingAddress_City")[0].value = city;
    $("#DoctorDetails_BillingAddress_PostalCode")[0].value = postalCode;
}
