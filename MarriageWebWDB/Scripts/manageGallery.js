function myfun(id) {

    if (!confirm("Are you sure?"))
        return;

    $.ajax({
        url: '/Account/DeleteGalleryPicture',
        data: { id: id },
        dataType: 'html',
        success: function (result) {
        document.getElementById(id).style.display = "none";
        }
    });
}
