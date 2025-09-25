$(function () {
    $('deleteBtn').click(function () {
        swal({
            title: "Are you sure?",
            text: "Are you sure you want to delete this record?",
            icon: "warning",
            buttons: true,
            dangerMode: true
        }).then((confirm) => {
            if (confirm) {
                var btn = $(this);
                var id = btn.data("id");
                $('#recordid').val(id);
                $('#deleteForm').submit();
            }
        });
    });
});