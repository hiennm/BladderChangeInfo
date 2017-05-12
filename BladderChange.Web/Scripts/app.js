/**
 * The JavaScipt source for loading & showing bladder information
 */

//API URL
var apiUrl = '/api/BladderData';

var updateInterval = 15 * 60; // 15 minutes = 900 seconds
var countDown = 900;

var dangerLevel = 20;
var warningLevel = 50;

/**
 * Setup and run initial tasks
 */
$(document).ready(function () {

    setInterval(ticker, 1000);
    loadBladderInfo();

});

function ticker() {
    //Update date time info at every tick
    updateDateTime();

    if (countDown > 0) {
        countDown--;
    } else {

        loadBladderInfo();
        //reset countDown
        countDown = updateInterval;
    }
}

/*
 * Update the date time and countdown value
 * @returns {} 
 */
function updateDateTime() {
    var min = Math.floor(countDown / 60);
    var sec = countDown % 60;

    $("#count-down").text(formatNumber(min) + ":" + formatNumber(sec));

/*
    var d = new Date();
    var sTime = (d.getMonth() + 1) + "/" + d.getDate() + "/" + d.getFullYear() + " " +
        d.getHours() + ":" + formatNumber(d.getMinutes()) + ":" + formatNumber(d.getSeconds());
    $("#current-date-time").text(sTime);
 */
}

/**
 * Loading bladder info from server and displaying in tabular format
 */
function loadBladderInfo() {
    $.getJSON(apiUrl)
        .done(function (infoList) {
            if (infoList === null || infoList === "null") {
                $("#bladder-info  tbody tr").remove();
                console.log('Cannot load data from server.');
            }
            else {
                infoList.sort(sortInfoByMachine);
                $("#bladder-info  tbody tr").remove();

                for (var i = 0; i < infoList.length; i++) {
                    var info = infoList[i];                    

                    var newRow = $('<tr/>').addClass("content-row");
                    if (i % 2 !== 1)
                        newRow.addClass('odd-row');

                    var remainLeft = info.BladderLimitLeft - info.BladderCountLeft;
                    var remainRight = info.BladderLimitRight - info.BladderCountRight;

                    var indicatorMachine = '';
                    var indicatorLeft = '';
                    var indicatorRight = '';

                    var minRemain = Math.min(remainLeft, remainRight);

                    if (minRemain <= dangerLevel)
                        indicatorMachine = 'danger';
                    else if (minRemain <= warningLevel)
                        indicatorMachine = 'warning';

                    if (remainLeft <= dangerLevel)
                        indicatorLeft = 'danger';
                    else if (remainLeft <= warningLevel)
                        indicatorLeft = 'warning';

                    if (remainRight <= dangerLevel)
                        indicatorRight = 'danger';
                    else if (remainRight <= warningLevel)
                        indicatorRight = 'warning';

                    newRow.append($('<td/>').text(info.MachineNo).addClass(indicatorMachine));
                    newRow.append($('<td/>').text(info.Size));
                    newRow.append($('<td/>').text(info.BladderNameLeft));
                    newRow.append($('<td/>').text(info.BladderLimitLeft));
                    newRow.append($('<td/>').text(info.BladderCountLeft).addClass(indicatorLeft));
                    newRow.append($('<td/>').text(remainLeft).addClass(indicatorLeft));
                    newRow.append($('<td/>').text(info.BladderCountRight).addClass(indicatorRight));
                    newRow.append($('<td/>').text(remainRight).addClass(indicatorRight));                    
                    newRow.append($('<td/>').text(formatDate(info.UpdDate)));

                    $("#bladder-info").append(newRow);
                }
            }
        })
        .fail(function (jqXHR, textStatus, err) {
            console.log(err);
        });

}

function formatDate(date) {
    var d = new Date(date);
    return d.getFullYear() + '-' + (d.getMonth() + 1) + '-' + d.getDay() + ' '
        + (d.getHours() < 10 ? '0' + d.getHours() : d.getHours()) + ':'
        + (d.getMinutes() < 10 ? '0' + d.getMinutes() : d.getMinutes());
}
/**
 * comparision function to sort bladder info items
 * @param {any} infoX
 * @param {any} infoY
 */
function sortInfoByRemain(infoX, infoY) {
    var minCountX = Math.min(infoX.BladderLimitLeft - infoX.BladderCountLeft, infoX.BladderLimitRight - infoX.BladderCountRight);
    var minCountY = Math.min(infoY.BladderLimitLeft - infoY.BladderCountLeft, infoY.BladderLimitRight - infoY.BladderCountRight);
    return minCountX - minCountY;
}

function sortInfoByMachine(infoX, infoY) {
    var x = infoX.MachineNo;
    var y = infoY.MachineNo;
    if (x > y)
        return 1;
    else if (x < y)
        return -1;
    else
        return 0;
}

/**
 * Formatting the integer number to construct the datetime string
 * @param {} n 
 * @returns {} 
 */
function formatNumber(n) {
    return n < 10 ? "0" + n : n;
}