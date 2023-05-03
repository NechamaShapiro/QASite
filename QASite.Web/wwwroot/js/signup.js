$(() => {
    $("#email").on('keyup', function () {

        const email = $(this).val();

        if (!isValidEmail(email)) {
            $(".btn-primary").prop('disabled', true);
            return;
        }

        //$.get(`/account/CheckEmailAvailability?email=${email}`, function (obj) {
        //    if (obj.isAvailable) {
        //        $(".btn-primary").prop('disabled', false);
        //    } else {
        //        $(".btn-primary").prop('disabled', true);
        //    }
        //});

        $.get(`/account/CheckEmailAvailability`, { email }, function (obj) {
            if (obj.isAvailable) {
                $(".btn-primary").prop('disabled', false);
            } else {
                $(".btn-primary").prop('disabled', true);
            }
        });
    });

    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }
});