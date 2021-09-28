// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function confirmDelete(isDeleteClicked) {
    var deleteSpan = 'deleteSpan';
    var confirmDeleteSpan = 'confirmDeleteSpan';

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}

function confirmDeleteById(id, isDeleteClicked) {
    var deleteSpan = 'deleteSpan_' + id;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + id;

    if (isDeleteClicked) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}
function showDateTimePicker(id) {
    $(".datepicker_"+id).datetimepicker({
        changeYear: true,
        changeMonth: true,
        lang: 'cs',
        formatTime: 'H:i:s',
        formatDate: 'd.m.Y',
        format: "d.m.Y H:i:s",
        defaultTime: '10:00'
    });
}

jQuery(document).ready(function ($) {
    $('.popup').on('click', function (event) {
        $(this).addClass('hidden');
    });
})