$(function () {
    let options = {
        locale: moment.locale("es", { week: { dow: 0 } }),
        // daysOfWeekDisabled: [0, 6],
        useCurrent: false,
        format: 'DD/MM/YYYY',
        tooltips: {
            today: "Hoy",
            clear: "Limpiar selección",
            close: "Cerrar el selector",
            selectMonth: "Seleccionar mes",
            prevMonth: "Mes anterior",
            nextMonth: "Mes siguiente",
            selectYear: "Seleccionar año",
            prevYear: "Año anterior",
            nextYear: "Año siguiente",
            selectDecade: "Seleccionar década",
            prevDecade: "Década anterior",
            nextDecade: "Década siguiente",
            prevCentury: "Siglo anterior",
            nextCentury: "Siglo siguiente",
            incrementHour: "Aumentar una hora",
            pickHour: "Seleccionar hora",
            decrementHour: "Disminuir una hora",
            incrementMinute: "Aumentar un minuto",
            pickMinute: "Seleccionar minuto",
            decrementMinute: "Disminuir un minuto",
            incrementSecond: "Aumentar un segundo",
            pickSecond: "Seleccionar segundo",
            decrementSecond: "Disminuir un segundo"
        }
    };

    $(".dtp-control").each(function (index) {
        let currentId = "#" + $(".dtp-control")[index].id;
        $(currentId).datetimepicker(options);
    });
});