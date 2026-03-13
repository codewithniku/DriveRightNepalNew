window.renderQuizChart = (labels, data) => {
    const ctx = document.getElementById('quizChart');
    if (!ctx) return;

    if (window.quizChartInstance) {
        window.quizChartInstance.destroy();
    }

    window.quizChartInstance = new Chart(ctx.getContext('2d'), {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Quiz Score (%)',
                data: data,
                fill: false,
                borderColor: '#517FA2',
                backgroundColor: '#517FA2',
                tension: 0,
                pointRadius: 5,
                pointBackgroundColor: '#517FA2',
                pointBorderColor: '#fff',
            }]
        },
        options: {
            responsive: false,           // ✅ turn off responsiveness
            maintainAspectRatio: false, // still keep aspect ratio control
            plugins: { legend: { display: false } },
            scales: { y: { beginAtZero: true, max: 100, ticks: { stepSize: 10 } } }
        }
    });
};