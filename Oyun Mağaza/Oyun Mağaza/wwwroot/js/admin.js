$(document).ready(function () {
    // Sidebar toggle
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    // Dropdown menu toggle
    $('.dropdown-toggle').on('click', function () {
        $(this).next('.collapse').collapse('toggle');
    });

    // Active menu item
    var path = window.location.pathname;
    var menuItems = $('#sidebar a');
    menuItems.each(function () {
        var href = $(this).attr('href');
        if (href && path.startsWith(href)) {
            $(this).addClass('active');
            $(this).parents('li').addClass('active');
            $(this).parents('.collapse').addClass('show');
        }
    });

    // DataTable initialization (if needed)
    if ($.fn.DataTable) {
        $('.table').DataTable({
            responsive: true,
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.24/i18n/Turkish.json'
            }
        });
    }

    // Form validation (if needed)
    if ($.fn.validate) {
        $('form').validate();
    }

    // Image preview
    $('input[type="file"]').change(function (e) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imagePreview').attr('src', e.target.result);
        }
        reader.readAsDataURL(this.files[0]);
    });

    // AJAX form submission
    $('form[data-ajax="true"]').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: new FormData(this),
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    if (response.redirect) {
                        window.location.href = response.redirect;
                    }
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('İşlem sırasında bir hata oluştu.');
            }
        });
    });
}); 