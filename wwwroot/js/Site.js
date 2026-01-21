/* ================= ADD USER FORM (SAFE PLACEHOLDER) ================= */
document.addEventListener("DOMContentLoaded", function () {

    const addForm = document.getElementById("addUserForm");
    const addBtn = document.getElementById("addBtn");

    if (addForm) {
        addForm.addEventListener("submit", function (e) {
            e.preventDefault();
            if (addBtn && addBtn.disabled) return;
            // Actual submit handled in OrgChart.cshtml
        });
    }

    /* ================= AUTO DISMISS ALERTS ================= */
    document.querySelectorAll(".auto-dismiss").forEach(alert => {
        setTimeout(() => {
            alert.classList.add("fade");
            setTimeout(() => alert.remove(), 400);
        }, 5000);
    });

    /* ================= HASH SCROLL ================= */
    if (window.location.hash) {
        const target = document.querySelector(window.location.hash);
        if (target) {
            target.scrollIntoView({ behavior: "smooth" });
        }
    }
});

/* ================= TEXTAREA COUNTER ================= */
function updateCounter(textarea) {
    const counter = textarea.nextElementSibling;
    if (!counter) return;
    counter.textContent = textarea.maxLength - textarea.value.length;
}

document.addEventListener("input", function (e) {
    if (!e.target.classList.contains("char-limit")) return;

    const max = 100;
    const counterId = e.target.getAttribute("data-counter");
    const counter = document.getElementById(counterId);

    if (!counter) return;

    const remaining = max - e.target.value.length;
    counter.textContent = remaining + " characters remaining";
});


function selectNode(id) {
    fetch(`/Users/GetDetails?id=${id}`)
        .then(r => r.text())
        .then(html => {
            document.getElementById("detailsPanel").innerHTML = html;
        });
}

// Removed conflicting global drag/drop handlers so orgchart.js can handle DnD consistently.
