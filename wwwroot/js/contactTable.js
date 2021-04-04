$(document).ready(function () {
    $('#contactsDatatable').dataTable({

        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/Contact/GetContactDatatableData",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [{
            "targets": [0],
            "visible": false,
            "searchable": false
        }],
        "columns": [
            { "data": "ContactId", "name": "Id", "autoWidth": true },
            { "data": "FirstName ", "name": "First Name", "autoWidth": true },
            { "data": "email", "name": "Email", "autoWidth": true },

        ]
    });
});