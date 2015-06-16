/*
	flot-example.js
*/

$(document).ready(init);


function init() {

    // Pie chart

    var data = [
	    { label: "1", data: 19.5, color: "orange" },
	    { label: "2", data: 4.5, color: "#9bc747" },
	    { label: "3", data: 36.6, color: "#208ed3" }
    ];

    $.plot($(".pie"), data, {
        series: {
            pie: {
                innerRadius: 0.5,
                show: true
            }
        }
    });

    ajax("../Admin/admin.ashx", "type=flot-TemHum", function (restext) {
        var result = JSON.parse(restext);

        var tem = new Array(23);
        var hum = new Array(23);
        var time=new Array(23)
        for (var i = 0; i < result.length; i++)
        {
            tem[i] = parseInt(result[i].Data.substr(2, 2), 16);
            hum[i] = parseInt(result[i].Data.substr(4, 2), 16);
            var time0 = new Date(result[i].Time);
            time[i] = time0.getMinutes().toString();
            
        }

        $.plot($("#flot-example-2"),
        [
                {
                    label: "Temperature",
                    color: "orange",
                    shadowSize: 0,
                    data: [[0, tem[0]], [1, tem[1]], [2, tem[2]], [3, tem[3]], [4, tem[4]],
                           [5, tem[5]], [6, tem[6]], [7, tem[7]], [8, tem[8]], [9, tem[9]],
                           [10, tem[10]], [11, tem[11]], [12, tem[12]], [13, tem[13]], [14, tem[14]],
                           [15, tem[15]], [16, tem[16]], [17, tem[17]], [18, tem[18]], [19, tem[19]],
                           [20, tem[20]], [21, tem[21]], [22, tem[22]], [23, tem[23]]],
                    lines: { show: true },
                    points: { show: true }
                }
        ],
            {
                xaxis: {
                    ticks: [
                        [0, time[0]],
                        [1, time[1]],
                        [2, time[2]],
                        [3, time[3]],
                        [4, time[4]],
                        [5, time[5]],
                        [6, time[6]],
                        [7, time[7]],
                        [8, time[8]],
                        [9, time[9]],
                        [10, time[10]],
                        [11, time[11]],
                        [12, time[12]],
                        [13, time[13]],
                        [14, time[14]],
                        [15, time[15]],
                        [16, time[16]],
                        [17, time[17]],
                        [18, time[18]],
                        [19, time[19]],
                        [20, time[20]],
                        [21, time[21]],
                        [22, time[22]],
                        [23, time[23]]
                    ]
                },

                grid: {
                    borderWidth: 0,
                    color: "#aaa",
                    clickable: "true"
                }
            }
        );
        $.plot($("#flot-example-1"),
        [
                {
                    label: "Humidity",
                    color: "#9bc747",
                    shadowSize: 0,
                    data: [[0, hum[0]], [1, hum[1]], [2, hum[2]], [3, hum[3]], [4, hum[4]],
                           [5, hum[5]], [6, hum[6]], [7, hum[7]], [8, hum[8]], [9, hum[9]],
                           [10, hum[10]], [11, hum[11]], [12, hum[12]], [13, hum[13]], [14, hum[14]],
                           [15, hum[15]], [16, hum[16]], [17, hum[17]], [18, hum[18]], [19, hum[19]],
                           [20, hum[20]], [21, hum[21]], [22, hum[22]], [23, hum[23]]
                    ],
                    lines: { show: true },
                    points: { show: true }
                }
        ],
            {
                xaxis: {
                    ticks: [
                       [0, time[0]],
                        [1, time[1]],
                        [2, time[2]],
                        [3, time[3]],
                        [4, time[4]],
                        [5, time[5]],
                        [6, time[6]],
                        [7, time[7]],
                        [8, time[8]],
                        [9, time[9]],
                        [10, time[10]],
                        [11, time[11]],
                        [12, time[12]],
                        [13, time[13]],
                        [14, time[14]],
                        [15, time[15]],
                        [16, time[16]],
                        [17, time[17]],
                        [18, time[18]],
                        [19, time[19]],
                        [20, time[20]],
                        [21, time[21]],
                        [22, time[22]],
                        [23, time[23]]
                    ]
                },

                grid: {
                    borderWidth: 0,
                    color: "#aaa",
                    clickable: "true"
                }
            }
        );
    });
}
