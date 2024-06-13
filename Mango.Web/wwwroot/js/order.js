let dataTable
function loadDataTable() {
    dataTable = $("tblData").DataTable({
        "ajax": { url: "/order/GetAll" },
        "columns": [
            {data:'orderheaderid',"width":"5%"},
            {data:'email',"width":"25%"},
            //{ data: 'orderheaderid', "width": 5 %},
        ]
    })
}