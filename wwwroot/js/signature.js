// wwwroot/js/signature.js
let canvas, ctx, drawing = false;

window.signatureInterop = {
    init(canvasId) {
        canvas = document.getElementById(canvasId);
        ctx = canvas.getContext('2d');
        ctx.strokeStyle = '#000';
        ctx.lineWidth = 2;
        ctx.lineCap = 'round';

        canvas.addEventListener('mousedown', e => { drawing = true; ctx.beginPath(); ctx.moveTo(pos(e).x, pos(e).y); });
        canvas.addEventListener('mousemove', e => { if (!drawing) return; ctx.lineTo(pos(e).x, pos(e).y); ctx.stroke(); });
        canvas.addEventListener('mouseup', () => drawing = false);
        canvas.addEventListener('mouseleave', () => drawing = false);

        // Touch support
        canvas.addEventListener('touchstart', e => { e.preventDefault(); drawing = true; ctx.beginPath(); ctx.moveTo(pos(e.touches[0]).x, pos(e.touches[0]).y); });
        canvas.addEventListener('touchmove', e => { e.preventDefault(); if (!drawing) return; ctx.lineTo(pos(e.touches[0]).x, pos(e.touches[0]).y); ctx.stroke(); });
        canvas.addEventListener('touchend', () => drawing = false);
    },
    clear(canvasId) {
        canvas = document.getElementById(canvasId);
        canvas.getContext('2d').clearRect(0, 0, canvas.width, canvas.height);
    },
    getDataUrl(canvasId) {
        return document.getElementById(canvasId).toDataURL('image/png');
    },
    isEmpty(canvasId) {
        const c = document.getElementById(canvasId);
        const data = c.getContext('2d').getImageData(0, 0, c.width, c.height).data;
        return !data.some(v => v !== 0);
    }
};

window.loginRequest = async function(username, password) {
    const response = await fetch('/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    });

    return response.ok;
};

function pos(e) {
    const rect = canvas.getBoundingClientRect();
    return { x: (e.clientX ?? e.pageX) - rect.left, y: (e.clientY ?? e.pageY) - rect.top };
}