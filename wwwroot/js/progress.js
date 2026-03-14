window.renderQuizChart = (labels, scores) => {
    const canvas = document.getElementById('quizChart');
    if (!canvas) return;

    const ctx = canvas.getContext('2d');

    if (window.myQuizChart) {
        window.myQuizChart.destroy();
    }

    window.myQuizChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Score %',
                data: scores,
                borderColor: '#5686E1',
                backgroundColor: 'rgba(86, 134, 225, 0.1)',
                tension: 0.3,
                fill: true
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false, // CRITICAL: This stops the stretching
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    max: 100
                }
            }
        }
    });
};