if (typeof BeerPongTracker !== "object") { BeerPongTracker = {}; }

BeerPongTracker.helpers = (function () {
    var _teamId = 0;
    var _playerId = 0;

    var _hidePlayerNameHelper = function () {
        if ($(".player-name-helper").length > 0) {
            $(".player-name-helper-overlay").remove();
            $(".player-name-helper").remove();
        }
    };

    var _helperOnTextbox = function(textBox) {
        if ($(textBox).val().length <= 3) {
            return;
        } else {
            var query = $(textBox).val();
            _teamId = $(textBox).attr("teamid");
            _playerId = $(textBox).attr("playerid");

            $.ajax({
                method: "GET",
                success: _playerNameHelperSuccess,
                error: _playerNameHelperError,
                url: "/Home/PlayerNameHelper?query=" + query + "&t=" + _teamId + "&p=" + _playerId
            });
        }
    };

    var _clearHidden = function (textBox) {
        var teamId = $(textBox).attr("teamid");
        var playerId = $(textBox).attr("playerid");

        var hiddenEl = _getHidden(playerId, teamId);

        $(hiddenEl).val("");
    };

    var _getHidden = function(playerId, teamId){
        var pagePlayerIdTextBoxName = "team-input__text--hidden-db-player-id--team-" + teamId + "-player-" + playerId;
        return $("." + pagePlayerIdTextBoxName);
    };

    var _init = function () {
        $(".team-input__text").unbind().on("keyup", function () {
            _clearHidden(this);
            _helperOnTextbox(this);
        });

        $('.team-input__text-wrapper').on('click', '.player-name-helper-overlay', function () {
            _hidePlayerNameHelper();
        });

        $(".team-input__text").focusin(function () {
            _helperOnTextbox(this);
        });

        $('.team-input__text-wrapper').on('click', '.player-name-helper__table__row', function () {
            var dbPlayerId = $(this).attr("playerid");
            var dbPlayerName = $(this).attr("playername");

            var playerNameHelper = $(this).closest(".player-name-helper");

            var elPlayerId = $(playerNameHelper).attr("playerid");
            var elTeamId = $(playerNameHelper).attr("teamid");

            var pagePlayerNameTextBoxName = "team-input__text--team-" + elTeamId + "-player-" + elPlayerId;
            
            var pagePlayerNameTextBox = $("." + pagePlayerNameTextBoxName);

            $(pagePlayerNameTextBox).val(dbPlayerName);

            var hiddenEl = _getHidden(elPlayerId, elTeamId);

            $(hiddenEl).val(dbPlayerId);

            _hidePlayerNameHelper();
        });
    };

    var _playerNameHelperSuccess = function (result) {
        var elementName = "team-input__text--team-" + _teamId + "-player-" + _playerId + "-wrapper";

        _hidePlayerNameHelper();

        $("." + elementName).append(result);

        _teamId = 0;
        _playerId = 0;
    };

    var _playerNameHelperError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    return {
        init: _init
    };
})();

BeerPongTracker.cupSelector = (function () {
    var _init = function () {
        $(".number-of-cups-selector__selection").unbind().click(function () {
            _select(this);
        });
    };

    var _select = function (cupSelector) {
        $(".number-of-cups-selector__selection").each(function () {
            $(this).removeClass("number-of-cups-selector__selection--selected");
        });

        $(cupSelector).addClass("number-of-cups-selector__selection--selected");

        var numerOfCups = $(cupSelector).attr("cups");

        $(".number-of-cups-selector__hidden").val(numerOfCups);
    };

    return {
        init: _init
    };
})();

BeerPongTracker.startGameButton = (function () {
    var _init = function () {
        $(".button--start-game").unbind().click(function () {
            var numerOfCups = $(".number-of-cups-selector__hidden").val();
            
            var teams = new Array();

            for (i = 1; i <= 4; i++) {
                var topTextBoxName = "team-input__text--team-" + i + "-player-1";
                
                if ($("." + topTextBoxName).val().length > 0) {
                    teamIndex = i - 1;
                    teams[teamIndex] = {Players : new Array()};

                    var teamGroupName = "team-input__text--team-" + i;

                    $("." + teamGroupName).each(function () {
                        var playerId = $(this).attr("playerid");
                        var playerIndex = playerId - 1;

                        var hiddenElName = "team-input__text--hidden-db-player-id--team-" + i + "-player-" + playerId
                        var hiddenEl = $("." + hiddenElName);

                        if ($(hiddenEl).val() > 0) {
                            var dbPlayerId = $(hiddenEl).val();
                            teams[teamIndex].Players[playerIndex] = { PlayerId: dbPlayerId }
                        } else if ($(this).val().length > 0) {
                            teams[teamIndex].Players[playerIndex] = { Name: $(this).val() }
                        }
                    });
                }
            }

            var jsonData = {
                NumberOfCups: numerOfCups,
                Teams : teams
            }

            $.ajax({
                contentType: "application/json",
                data: JSON.stringify(jsonData),
                method: "POST",
                success: _startGameSuccess,
                error: _startGameError,
                url: "/Game/StartGame"
            });
        });
    };

    var _startGameSuccess = function (result) {
        alert(result.GameId);
    };

    var _startGameError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown)
    };

    return {
        init: _init
    };
})();

BeerPongTracker.controlling = (function () {
    var _init = function () {
        $(".cup-cover .cup").unbind().click(function () {
            var cupId = $(this).attr('cupid');
            var teamId = $(this).attr('teamid');
            var gameId = $(this).parent(".cup-cover").attr('gameid');

            var jsonData =
            {
                GameId: gameId,
                TeamId: teamId,
                CupId: cupId,
            };

            $.ajax({
                contentType: "application/json",
                data: JSON.stringify(jsonData),
                method: "POST",
                success: _cupCoverSuccess,
                error: _cupCoverError,
                url: "/Game/CupSwitch"
            });
        });
    };

    var _cupCoverSuccess = function (result) {
        $(result.Teams).each(function () {
            var teamId = this.TeamId;
            var teamHealthElement = $("#team-" + this.TeamId + "-health");
            teamHealthElement.css({ width: this.Health + "%" });

            if (this.Health < 25) {
                teamHealthElement.removeClass("team-info__health__container__remaining--green");
                teamHealthElement.addClass("team-info__health__container__remaining--red");
            } else {
                teamHealthElement.removeClass("team-info__health__container__remaining--red");
                teamHealthElement.addClass("team-info__health__container__remaining--green");
            }

            $(this.CupStats).each(function () {
                var cupElementId = "team-" + teamId + "-cup-" + this.CupId;
                var cupElement = $("#" + cupElementId);
                if (this.Active) {
                    cupElement.removeClass("empty");
                    cupElement.addClass("full");
                } else {
                    cupElement.removeClass("full");
                    cupElement.addClass("empty");
                }
            });
        });
    }

    var _cupCoverError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    return {
        init: _init
    };
})();