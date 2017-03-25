BeerPongTracker.dialofdoom = (function () {
    var _menuButtonSetup = function () {
        $("body").on("click", ".button--dial-of-doom", function () {
            $.ajax({
                contentType: "application/json",
                method: "GET",
                success: function (data) {
                    $(".screen--1").html(data);
                },
                error: function (error) {
                    alert(error);
                },
                url: "/Home/DialOfDoom"
            });
        });
    }

    var _showWheelButtonSetup = function () {
        $("body").on("click", ".button--dial-of-doom-show-wheel", function () {
            $("html").addClass("body--dojo");
            $(".dial-of-doom-input").hide();
            var names = [];
            $(".dial-of-doom-entry").each(function () {
                if ($(this).val() != '') {
                    names.push($(this).val());
                }
            });
            var myWheel = SPINWHEEL.wheelOfDestiny('dialOfDoomContainer', names);

            $("body").on("click", "#dialOfDoomContainer", function () {
                myWheel.Start();
            });
        });
    }

    var _init = function () {
        _menuButtonSetup();
        _showWheelButtonSetup();
    };

    return {
        init: _init,
    };
})();