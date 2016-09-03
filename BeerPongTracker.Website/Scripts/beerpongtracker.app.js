if (typeof BeerPongTracker !== "object") { BeerPongTracker = {}; }

BeerPongTracker.main = (function () {
    var _init = function () {
        BeerPongTracker.mainMenu.init();
        BeerPongTracker.helpers.init();
        BeerPongTracker.cupSelector.init();
        BeerPongTracker.startGameButton.init();

        _checkForExistingGame();
    };

    var _checkForExistingGame = function () {
        if (location.hash.match("^#c")) {
            BeerPongTracker.global.setControlling(true);
            var gameId = location.hash.replace("#c", "");
            BeerPongTracker.global.displayLoadingScreen();
            BeerPongTracker.global.loadGame(gameId);
        }

        if (location.hash.match("^#w")) {
            BeerPongTracker.global.setControlling(false);
            var gameId = location.hash.replace("#w", "");
            BeerPongTracker.global.displayLoadingScreen();
            BeerPongTracker.global.loadGame(gameId);
        }
    };

    return {
        init: _init
    };
})();

BeerPongTracker.global = (function () {
    var _controlling = false;

    var _isPc = $(window).width() > 1000;

    var _getIsPc = function () {
        return _isPc;
    };

    var _gameId = 0;

    var _numberOfTeams = 0;

    var _getNumberOfTeams = function () {
        return _numberOfTeams;
    };

    var _setNumberOfTeams = function (numberOfTeams) {
        _numberOfTeams = numberOfTeams;
    };

    var _getGameId = function () {
        return _gameId;
    };

    var _lastUpdateSignature = "";

    var _getLastUpdateSignature = function () {
        return _lastUpdateSignature;
    };

    var _setLastUpdateSignature = function (lastUpdateSignature) {
        _lastUpdateSignature = lastUpdateSignature;
    };

    var _setControlling = function(controllingBool) {
        _controlling = controllingBool;
    };

    var _getControlling = function () {
        return _controlling;
    };

    var _getGuid = function () {
        return Math.floor(Math.random() * 10000000000000001);
    }

    var _displayLoadingScreen = function () {
        $(".screen").hide();
        $("body").append("<div class=\"loading\"><img src=\"/Content/Images/loading.gif\" /></div>");
    };

    var _hideLoadingScreen = function () {
        $(".loading").remove();
    };

    var _loadGame = function (gameId) {
        _gameId = gameId;

        var jsonData = {
            GameId: gameId,
            Controlling: BeerPongTracker.global.getControlling(),
            IsPc: BeerPongTracker.global.getIsPc()
        };

        $.ajax({
            contentType: "application/json",
            method: "POST",
            data: JSON.stringify(jsonData),
            success: _getGameDataSuccess,
            error: _getGameDataError,
            url: "/Game/Build"
        });
    }

    var _getGameDataSuccess = function (result) {
        $(".screen--3").append(result);
        _setLastUpdateSignature($(".game-meta-info").attr("lus"));
        _setNumberOfTeams($(".game-meta-info").attr("not"));
        if (BeerPongTracker.global.getControlling()) {
            BeerPongTracker.controlling.init();
        } else {
            BeerPongTracker.watching.init();
        }
        _hideLoadingScreen();
        $(".screen--3").show(result);
    };

    var _getGameDataError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    return {
        displayLoadingScreen: _displayLoadingScreen,
        hideLoadingScreen: _hideLoadingScreen,
        loadGame: _loadGame,
        setControlling: _setControlling,
        getControlling: _getControlling,
        getIsPc: _getIsPc,
        getGameId: _getGameId,
        getLastUpdateSignature: _getLastUpdateSignature,
        setLastUpdateSignature: _setLastUpdateSignature,
        getNumberOfTeams: _getNumberOfTeams,
        setNumberOfTeams: _setNumberOfTeams,
        getGuid: _getGuid
    };
})();

BeerPongTracker.mainMenu = (function() {
    var _init = function () {
        $(".button--watch-game").click(function() {
            BeerPongTracker.global.displayLoadingScreen();
            BeerPongTracker.global.setControlling(false);
            var gameId = $(this).attr("gameid");
            location.hash = '#w' + gameId;
            BeerPongTracker.global.loadGame(gameId);
        });

        $(".button--create-game").click(function() {
            $(".screen--1").hide();
            $(".screen--2").show();
        });
    };

    return {
        init: _init
    };
})();

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
        if ($(textBox).val().length <= 2) {
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

        $(".team-input__text").click(function () {
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

        var numberOfCups = $(cupSelector).attr("cups");

        $(".number-of-cups-selector__hidden").val(numberOfCups);
    };

    return {
        init: _init
    };
})();

BeerPongTracker.controlling = (function () {
    var _init = function () {
        $(".cup-cover .cup").unbind().click(function () {
            if ($(this).hasClass("full")) {
                $(this).removeClass("full");
                $(this).addClass("empty");
            } else {
                $(this).removeClass("empty");
                $(this).addClass("full");
            }

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

        $("body").on("click", ".winner-prompt", function () {
            $(".winner-prompt").fadeOut();

            var winningTeamId = $(this).attr("teamid");

            BeerPongTracker.watching.declareWinner(winningTeamId);

            var jsonData = {
                WinningTeamId: winningTeamId,
                GameId: BeerPongTracker.global.getGameId()
            };

            $.ajax({
                contentType: "application/json",
                data: JSON.stringify(jsonData),
                method: "POST",
                success: function () { },
                error: function () { },
                url: "/Game/DeclareWinner"
            });
        });

        $("body").on("click", ".stop-music-button", function () {
            $(".stop-music-button").fadeOut();

            var gameId = BeerPongTracker.global.getGameId()
            var signature = "ew-" + BeerPongTracker.global.getGuid();

            $.ajax({
                contentType: "application/json",
                method: "GET",
                success: function () {
                    $(".screen--4").html("");
                    $(".screen--4").hide();
                    $(".screen--1").show();
                },
                error: function () { },
                url: "/Home/RegisterGenericEvent?sig=" + signature + "&gid=" + gameId
            });
        });

        var timeoutId = 0;

        $('body').mousedown(function () {
            timeoutId = setTimeout(_toggleContextMenu, 1000);
        }).bind('mouseup mouseleave', function () {
            clearTimeout(timeoutId);
        });

        var _toggleContextMenu = function () {
            if ($(".entrance-prompt").length > 0) {
                $(".entrance-prompt").remove();
                return;
            }

            var numberOfTeams = BeerPongTracker.global.getNumberOfTeams();

            for (teamId = 1; teamId <= numberOfTeams; teamId++) {
                var cupCoverElName = "cup-cover--team-" + teamId;
                var cupCoverEl = $("." + cupCoverElName);
                var positionStyle = _getCupCoverCentrePoint(BeerPongTracker.global.getNumberOfTeams(), teamId);
                cupCoverEl.prepend("<div state=\"1\" style=\"" + positionStyle + "\" class=\" button--green entrance-prompt\" teamid=\"" + teamId + "\"><span class=\"entrance-prompt--text\">PLAY ENTRANCE</span></div>");
            }
        };

        $("body").on("click", ".entrance-prompt", function () {
            if ($(this).attr("state") == "1") {
                $(this).removeClass("button--green");
                $(this).addClass("button--red");
                $(this).find(".entrance-prompt--text").html("STOP ENTRANCE");
                $(this).attr("state", "2");
                var gameId = BeerPongTracker.global.getGameId()
                var teamId = $(this).attr("teamid");
                var signature = "be-" + teamId + "-" + BeerPongTracker.global.getGuid();
                $.ajax({
                    contentType: "application/json",
                    method: "GET",
                    success: function () {
                    },
                    error: function () { },
                    url: "/Home/RegisterGenericEvent?sig=" + signature + "&gid=" + gameId
                });
            }
            else if ($(this).attr("state") == "2") {
                $(this).fadeOut();
                var gameId = BeerPongTracker.global.getGameId()
                var teamId = $(this).attr("teamid");
                var signature = "ee-" + teamId + "-" + BeerPongTracker.global.getGuid();
                $.ajax({
                    contentType: "application/json",
                    method: "GET",
                    success: function () {
                    },
                    error: function () { },
                    url: "/Home/RegisterGenericEvent?sig=" + signature + "&gid=" + gameId
                });
            }
        });
    };

    var _cupCoverSuccess = function (result) {
        if (BeerPongTracker.global.getIsPc())
        {
            BeerPongTracker.watching.updateBoard();
        }
        _checkForWinner(result);
    }

    var _checkForWinner = function (gameModel) {
        var deadTeamsCount = 0;
        $(gameModel.Teams).each(function () {
            if (this.Health < 1) {
                deadTeamsCount++;
            }
        });
        if (deadTeamsCount == (gameModel.Teams.length - 1)) {
            var winningTeamId = 0;
            $(gameModel.Teams).each(function () {
                if (this.Health >= 1) {
                    winningTeamId = this.TeamId;
                    return _promptForEndGame(winningTeamId);
                }
            });
        } else {
            _removeEndGamePrompt();
        }
    }

    var _promptForEndGame = function (winningTeamId) {
        var cupCoverElName = "cup-cover--team-" + winningTeamId;
        var cupCoverEl = $("." + cupCoverElName);
        var positionStyle = _getCupCoverCentrePoint(BeerPongTracker.global.getNumberOfTeams(), winningTeamId);
        if ($(".winner-prompt").length == 0) {
            cupCoverEl.prepend("<div style=\"" + positionStyle + "\" class=\" button--gold winner-prompt\" teamid=\"" + winningTeamId + "\"><span class=\"winner-prompt--text\">DECLARE WINNER?</span></div>");
        }
    };

    var _removeEndGamePrompt = function () {
        if ($(".winner-prompt").length > 0) {
            $(".winner-prompt").remove();
        };
    };

    var _cupCoverError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    var _getCupCoverCentrePoint = function(numberOfTeams, teamId) {
        var cupCoverCentrePointMap = new Array();
        cupCoverCentrePointMap["2-1"] = "179@130";
        cupCoverCentrePointMap["2-2"] = "179@-192";
        cupCoverCentrePointMap["3-1"] = "229@213";
        cupCoverCentrePointMap["3-2"] = "225@499";
        cupCoverCentrePointMap["3-3"] = "477@352";
        cupCoverCentrePointMap["4-1"] = "301@134";
        cupCoverCentrePointMap["4-2"] = "121@293";
        cupCoverCentrePointMap["4-3"] = "291@468";
        cupCoverCentrePointMap["4-4"] = "470@294";

        var topLeft = cupCoverCentrePointMap[numberOfTeams + "-" + teamId];
        var topLeftArr = topLeft.split("@");

        return "top:" + (topLeftArr[0] - 70) + "px;left:" + (topLeftArr[1] - 70) + "px;"
    }

    return {
        init: _init
    };
})();

BeerPongTracker.watching = (function () {
    var _init = function () {
        var gameId = BeerPongTracker.global.getGameId;

        _listenForChange();
    };

    var _listenForChange = function () {
        var jsonData = {
            GameId: BeerPongTracker.global.getGameId(),
            LastUpdateSignature: BeerPongTracker.global.getLastUpdateSignature()
        };

        $.ajax({
            contentType: "application/json",
            data: JSON.stringify(jsonData),
            method: "POST",
            success: _listenForChangeSuccess,
            error: _listenForChangeError,
            url: "/Game/ListenForChange"
        });
    };

    var _listenForChangeSuccess = function (result) {
        if (result.Updated) {
            BeerPongTracker.global.setLastUpdateSignature(result.LastUpdateSignature);
            _resolveChangeNotice(result.LastUpdateSignature);
        }

        _listenForChange();
    };

    var _resolveChangeNotice = function (updateSignature) {
        if (updateSignature.match("^cs-")) {
            _updateBoard();
        };

        if (updateSignature.match("^ew-")) {
            _endDeclareWinner();
        };

        if (updateSignature.match("^tw-")) {
            var frags = updateSignature.split("-");
            var winningTeamId = frags[1];

            _declareWinner(winningTeamId);
        }

        if (updateSignature.match("^be-")) {
            var frags = updateSignature.split("-");
            var teamId = frags[1];

            _startEntrance(teamId);
        }

        if (updateSignature.match("^ee-")) {
            _endEntrance();
        }
    };

    var _startEntrance = function (teamId) {
        var gameId = BeerPongTracker.global.getGameId();
        $.ajax({
            contentType: "application/json",
            method: "GET",
            success: function (result) {
                $(".screen--entrance").html(result);
                BeerPongTracker.entrance.init();
                $(".screen--3").hide();
                $(".screen--entrance").show();
            },
            error: function () { },
            url: "/Home/GetEntranceScreen?gameId=" + gameId + "&teamId=" + teamId
        });
    };

    var _endEntrance = function () {
        $(".screen--entrance").html("");
        $(".screen--entrance").hide();
        $(".screen--3").show();
    };

    var _declareWinner = function (winningTeamId) {
        var jsonData = {
            WinningTeamId: winningTeamId,
            GameId: BeerPongTracker.global.getGameId(),
            Controlling: BeerPongTracker.global.getControlling()
        };

        $.ajax({
            contentType: "application/json",
            data: JSON.stringify(jsonData),
            method: "POST",
            success: _declareWinnerSuccess,
            error: _declareWinnerError,
            url: "/Home/GetWinnerDetails"
        });
    };

    var _declareWinnerSuccess = function (result) {
        $(".screen--4").html(result);
        $(".screen--3").fadeOut(3000, function () {
            setTimeout(function () {
                $(".screen--4").fadeIn(1000);
                BeerPongTracker.winScreen.init();
            }, 3000);
        });
    };

    var _endDeclareWinner = function () {
        $(".screen--4").html("");
        $(".screen--4").hide();
        $(".screen--3").show();
    }

    var _declareWinnerError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    var _updateBoard = function () {
        $.ajax({
            contentType: "application/json",
            method: "GET",
            success: _updateBoardSuccess,
            error: _updateBoardError,
            url: "/Game/GetGame?gameId=" + BeerPongTracker.global.getGameId()
        });
    }

    var _updateBoardSuccess = function (result) {
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
    };

    var _updateBoardError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    var _listenForChangeError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown);
    };

    return {
        init: _init,
        updateBoard: _updateBoard,
        declareWinner: _declareWinner,
        endDeclareWinner: _endDeclareWinner
    };
})();

BeerPongTracker.startGameButton = (function () {
    var _init = function () {
        $(".button--start-game").unbind().click(function () {
            BeerPongTracker.global.displayLoadingScreen();

            var numberOfCups = $(".number-of-cups-selector__hidden").val();

            var teams = new Array();

            for (i = 1; i <= 4; i++) {
                var topTextBoxName = "team-input__text--team-" + i + "-player-1";

                if ($("." + topTextBoxName).val().length > 0) {
                    teamIndex = i - 1;
                    teams[teamIndex] = { Players: new Array() };

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
                NumberOfCups: numberOfCups,
                Teams: teams
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
        var gameId = result.GameId;

        location.hash = '#c' + gameId;

        BeerPongTracker.global.setControlling(true);

        BeerPongTracker.global.loadGame(gameId);
    };

    var _startGameError = function (xhr, textStatus, errorThrown) {
        console.log(errorThrown)
    };

    return {
        init: _init
    };
})();

BeerPongTracker.winScreen = (function () {
    var _init = function () {
        setInterval(function () { _flash() }, 250);

        setInterval(function () { _flash2() }, 2000);

        setTimeout(function () {
            $(".name-plate-view-wrapper").show();
            $(".name-plate-view-wrapper").addClass("flipInX animated");
        }, 7000);
    };

    var _flash = function () {
        var flashingEl = $(".screen--fixed--outer");
        if (flashingEl.hasClass("orange-background")) {
            flashingEl.removeClass("orange-background");
            flashingEl.addClass("orange2-background");
        } else {
            flashingEl.addClass("orange-background");
            flashingEl.removeClass("orange2-background");
        }
    };

    var _flash2 = function () {
        setTimeout(function () {
            var flashingEl = $(".screen--fixed--inner");
            flashingEl.addClass("camera-flash");
            setTimeout(function () {
                flashingEl.removeClass("camera-flash");
            }, 100);
        }, Math.floor(Math.random() * 2000) + 1);
    };

    return {
        init: _init
    };
})();

BeerPongTracker.entrance = (function () {
    var _switch = 0;

    var _screenEl;

    var _textEl;

    var _imageEl;

    var flashInterval;

    var animationInterval;

    var _init = function () {
        clearInterval(flashInterval);
        clearInterval(animationInterval);

        var _flash = function (palette) {
            flashInterval = setInterval(function () {
                if (_switch == 0) {
                    _switch = 1;
                    $(_screenEl).attr("style", "background:#" + palette.backBase + ";");
                } else {
                    _switch = 0;
                    $(_screenEl).attr("style", "background:#" + palette.backLight + ";");
                }
            }, flashRate);
        };

        var _colorText = function (palette, font) {
            $(".screen--entrance__name").attr("style", "color:#" + palette.textBase + ";font-family:" + font + ";");
        }

        var _animation1 = function (palette, textAnimation, imageAnimation, flashRate, font) {
            var _animation1Switch = 0;

            _flash(palette);
            _colorText(palette, font);

            $(_textEl).removeClass("hide");
            $(_textEl).addClass(textAnimation.fadeIn);

            animationInterval = setInterval(function () {
                if (_animation1Switch == 0) {
                    _animation1Switch = 1;
                    $(_textEl).addClass(textAnimation.fadeOut);
                    $(_imageEl).fadeIn(500);
                } else {
                    _animation1Switch = 0;
                    $(_imageEl).fadeOut(500);
                    $(_textEl).removeClass(textAnimation.fadeOut);
                    $(_textEl).addClass(textAnimation.fadeIn);
                    _animation1Switch = 0;
                }
            }, 5000);
        }

        var palettes = new Array();
        palettes.push({
            backBase: "1F5300",
            backLight: "2E7C00",
            backDark: "143500",
            textBase: "29013F",
            textLight: "3F045F",
            textDark: "1A0129"
        });
        palettes.push({
            backBase: "5A1BFF",
            backLight: "C4ADFF",
            backDark: "200072",
            textBase: "FF6C00",
            textLight: "FFCAA4",
            textDark: "A74700"
        });
        palettes.push({
            backBase: "FFD100",
            backLight: "FFEEA4",
            backDark: "A78800",
            textBase: "0EA5FF",
            textLight: "A9DFFF",
            textDark: "00436A"
        });
        palettes.push({
            backBase: "FE0028",
            backLight: "FFA4B2",
            backDark: "BE001E",
            textBase: "FF7400",
            textLight: "FFCDA4",
            textDark: "C95B00"
        });
        palettes.push({
            backBase: "7A10FF",
            backLight: "CAA1FE",
            backDark: "330074",
            textBase: "FFE900",
            textLight: "FFF699",
            textDark: "AB9D00"
        });
        palettes.push({
            backBase: "00F391",
            backLight: "FFFFFF",
            backDark: "005F39",
            textBase: "FF4C00",
            textLight: "FFFFFF",
            textDark: "8C2A00"
        });
        palettes.push({
            backBase: "DECC00",
            backLight: "FFEB00",
            backDark: "A99F28",
            textBase: "2D5560",
            textLight: "4D7580",
            textDark: "173C47"
        });
        palettes.push({
            backBase: "FF0023",
            backLight: "FFA4B1",
            backDark: "9F0016",
            textBase: "66079E",
            textLight: "8303CF",
            textDark: "521675"
        });
        palettes.push({
            backBase: "1EFF12",
            backLight: "B4FFB0",
            backDark: "079400",
            textBase: "DE1500",
            textLight: "FF1800",
            textDark: "B12516"
        });
        palettes.push({
            backBase: "2D0C6E",
            backLight: "370A89",
            backDark: "200949",
            textBase: "A18E01",
            textLight: "C9B100",
            textDark: "6B5E04"
        });
        palettes.push({
            backBase: "017C16",
            backLight: "009A1B",
            backDark: "035211",
            textBase: "A10D01",
            textLight: "C90F00",
            textDark: "6B0C04"
        });
        palettes.push({
            backBase: "FFBEE1",
            backLight: "FFFFFF",
            backDark: "FF78C2",
            textBase: "57BFAB",
            textLight: "B3F5E8",
            textDark: "247968"
        });
        palettes.push({
            backBase: "FFD900",
            backLight: "FFF1A4",
            backDark: "A78E00",
            textBase: "FF000D",
            textLight: "FFA4A9",
            textDark: "A40008"
        });
        palettes.push({
            backBase: "3C21FF",
            backLight: "B8B0FF",
            backDark: "0E0074",
            textBase: "FF000D",
            textLight: "FFA4A9",
            textDark: "A40008"
        });
        palettes.push({
            backBase: "211C1D",
            backLight: "B54962",
            backDark: "6C051D",
            textBase: "BAC34E",
            textLight: "717446",
            textDark: "6C7405"
        });
        palettes.push({
            backBase: "62512A",
            backLight: "8B6100",
            backDark: "3A3834",
            textBase: "311F42",
            textLight: "32075E",
            textDark: "252327"
        });
        palettes.push({
            backBase: "FEF893",
            backLight: "F7F6E4",
            backDark: "FFF100",
            textBase: "AFF44A",
            textLight: "C7FA7E",
            textDark: "98ED1C"
        });
        palettes.push({
            backBase: "6B0050",
            backLight: "880066",
            backDark: "3A002C",
            textBase: "688A00",
            textLight: "85B000",
            textDark: "384A00"
        });
        palettes.push({
            backBase: "186DFF",
            backLight: "ACCBFF",
            backDark: "00296F",
            textBase: "FF7400",
            textLight: "FFCDA4",
            textDark: "A74C00"
        });
        palettes.push({
            backBase: "8506A9",
            backLight: "A54EBE",
            backDark: "500366",
            textBase: "D5F800",
            textLight: "E5FA61",
            textDark: "819600"
        });

        var textAnimations = new Array();
        textAnimations.push({
            fadeIn: "zoomIn",
            fadeOut: "zoomOut",
        });
        textAnimations.push({
            fadeIn: "slideInLeft",
            fadeOut: "slideOutRight",
        });
        textAnimations.push({
            fadeIn: "rotateIn",
            fadeOut: "rotateOut",
        });
        textAnimations.push({
            fadeIn: "bounceIn",
            fadeOut: "bounceOut",
        });
        textAnimations.push({
            fadeIn: "flipInY",
            fadeOut: "flipOutY",
        });
        textAnimations.push({
            fadeIn: "lightSpeedIn",
            fadeOut: "lightSpeedOut",
        });
        textAnimations.push({
            fadeIn: "rollIn",
            fadeOut: "rollOut",
        });
        textAnimations.push({
            fadeIn: "fadeInDown",
            fadeOut: "fadeOutDown",
        });
        textAnimations.push({
            fadeIn: "bounceInLeft",
            fadeOut: "bounceOutRight",
        });
        textAnimations.push({
            fadeIn: "zoomInUp",
            fadeOut: "zoomOutDown",
        });

        var fonts = new Array("Baloo Chettan", "Amatic SC", "Pacifico", "Patua One", "Bangers", "Black Ops One", "Monoton", "Audiowide", "Special Elite", "Bungee Inline", "Bubblegum Sans", "Fontdiner Swanky", "Corben", "Cinzel Decorative", "Bowlby One SC", "Expletus Sans", "Rammetto One", "Faster One", "Henny Penny", "Irish Grover");

        var palette = palettes[(1 + Math.floor(Math.random() * palettes.length)) - 1];

        var textAnimation = textAnimations[(1 + Math.floor(Math.random() * textAnimations.length)) - 1];

        var imageAnimation = textAnimations[(1 + Math.floor(Math.random() * textAnimations.length)) - 1];

        var font = fonts[(1 + Math.floor(Math.random() * fonts.length)) - 1];

        var flashRate = (2 + (1 + Math.floor(Math.random() * 5))) * 70;

        _screenEl = $(".screen--entrance");

        _textEl = $(".screen--entrance__name");

        _imageEl = $(".screen--entrance__picture");

        _animation1(palette, textAnimation, imageAnimation, flashRate, font);
    };

    return {
        init: _init
    };
})();
