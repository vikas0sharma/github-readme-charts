function drawChart(chartJson) {
    const chartOptions = JSON.parse(chartJson);
    chartOptions.chart.events = {
        load: function () {
            const svg = document.getElementsByTagName('svg')[0];
            const width = svg.getAttribute('width');
            const height = svg.getAttribute('height');
            const serialized = new XMLSerializer().serializeToString(svg);
            const encodedData = window.btoa(unescape(encodeURIComponent(serialized)));
            const canvas = document.createElement('canvas');
            const ctx = canvas.getContext('2d');
            const img = new Image();
            img.setAttribute('src', 'data:image/svg+xml;base64,' + encodedData);
            img.onload = function () {
                canvas.width = width;
                canvas.height = height;
                ctx.drawImage(img, 0, 0);
                document.getElementById('image-data').setAttribute('data', canvas.toDataURL('image/png'));
            }
        }
    };
    chartOptions.series.map(s => s.animation = false);
    console.log(chartOptions);
    Highcharts.chart('container', chartOptions);

}