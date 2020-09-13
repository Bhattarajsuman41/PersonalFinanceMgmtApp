
var ctxL = document.getElementById("lineChart").getContext('2d');

var myLineChart = new Chart(ctxL, {
    type: 'line',
    data: {
        labels: ["January", "February", "March", "April", "May", "June", "July","August","September","October","November","December"],
        datasets: [{
            label: "Bills&Utlities",
            data: [],
            backgroundColor: [
                'rgba(105, 0, 132, .2)',
            ],
            borderColor: [
                'rgba(200, 99, 132, .7)',
            ],
            borderWidth: 2
        },
        {
            label: "Shopping",
            data: [],
            backgroundColor: [
                'rgba(0, 137, 132, .2)',
            ],
            borderColor: [
                'rgba(0, 10, 130, .7)',
            ],
            borderWidth: 2
            },
            {
                label: "Food&Drinks",
                data: [],
                backgroundColor: [
                    'rgba(0, 10, 130, .7)',
                ],
                borderColor: [
                   
                    'rgba(0, 137, 132, .2)',
                ],
                borderWidth: 2
            },
            {
                label: "Other",
                data: [],
                backgroundColor: [
                    'rgba(50, 110, 30, .7)',
                ],
                borderColor: [
                    'rgba(250, 50, 100, .2)',
                ],
                borderWidth: 2
            }
        ]
    },
    options: {
        responsive: true
    },
    
});


