let dataTable

$(document).ready(function () {
    loadDataTable();
});
function loadDataTable() {
    console.log("Data table")
    dataTable = $("#tblData").DataTable({
        "ajax": { url: "/order/GetAll" },
        "columns": [
            {data:'orderHeaderId',"width":"5%"},
            {data:'email',"width":"25%"},
            { data: 'name', "width": "20%"},
            { data: 'phone', "width": "10%"},
            { data: 'status', "width": "10%"},
            { data: 'totalAmount', "width": "10%" },
            {
                data: "orderHeaderId",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/order/orderDetail?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                width:"10%"
            }
        ]
    })
}