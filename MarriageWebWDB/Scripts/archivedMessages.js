function DeleteMessage() {

    $('input[name="toDelete"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            $.ajax({
                url: '/Home/DeleteMessage',
                data: { id: this.id },
                dataType: 'html',
                success: function (result) { }
            });
            $(this).closest("tr").remove();;
        }
    });
}

$(function () {
    $("#deleteButton").click(function () {
        DeleteMessage();

        var length = $('#table tbody tr').length;
        if (length == 0) {
            document.getElementById("deleteButton").style.display = "none";
            document.getElementById("table").style.display = "none";
            $("#noMessages").text('No archived messages.');
        }
    })
});

$('#select-all').click(function (event) {
    if (this.checked) {
        $(':checkbox').each(function () {
            this.checked = true;
        });
    } else {
        $(':checkbox').each(function () {
            this.checked = false;
        });
    }
});

function sortTable(n) {
    var table, rows, switching, i, x, y, shouldSwitch, dir, switchcount = 0;
    table = document.getElementById("table");
    switching = true;
    dir = "asc";
    while (switching) {
        switching = false;
        rows = table.rows;
        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;
            x = rows[i].getElementsByTagName("TD")[n];
            y = rows[i + 1].getElementsByTagName("TD")[n];
            if (dir == "asc") {
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            } else if (dir == "desc") {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            }
        }
        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
            switchcount++;
        } else {
            if (switchcount == 0 && dir == "asc") {
                dir = "desc";
                switching = true;
            }
        }
    }
}