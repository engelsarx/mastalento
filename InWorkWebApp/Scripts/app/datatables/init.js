$(function () {
    var settings = {
        paging: false,
        responsive: true,
        language: {
            "decimal": "",
            "emptyTable": "No existen datos",
            "info": "Mostrando _START_ de _END_ de _TOTAL_ registros",
            "infoEmpty": "Mostrando 0 de 0 de 0 registros",
            "infoFiltered": "(filtered from _MAX_ total entries)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ registros",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "No se encontraron coincidencias",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            },
            "aria": {
                "sortAscending": ": activar para ordenar la columna de manera ascendente",
                "sortDescending": ": activar para ordenar la columna de manera descendente"
            }
        },
        "columnDefs": [{ "targets": 6, "orderable": false, "visible": true }]
    };
    var table = $('#aggregateValuesTable').DataTable(settings);
    $("input[type='checkbox']").each(function () {
        $(this).prop("checked", true);
    });
    $(".toggle-vis").on("change", function () {
        var column = table.column($(this).attr('data-column'));
        column.visible($(this).prop("checked"));
    });
});
//# sourceMappingURL=init.js.map