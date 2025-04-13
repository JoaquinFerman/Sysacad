import { verifyToken } from "./verifyToken.js";

const token = document.cookie.replace(/(?:(?:^|.*;\s*)token\s*=\s*([^;]*).*$)|^.*$/, "$1");

const init = async () => {
    if (!token) {
        alert("Token not found, please re login.");
        window.location.href = "login.html";
    } else if (!(await verifyToken())) {
        alert("Token expired, please re login.");
        window.location.href = "login.html";
    }
    
    try {
        const response = await fetch("http://localhost:5000/api/v0/name", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
        });
        if (response.ok) {
            const result = await response.json();
            document.getElementById("nameBox").textContent = result.surname + ", " + result.name;
        } else {
            document.getElementById("nameBox").textContent = "Error";
        }
    } catch (error) {
        document.getElementById("nameBox").textContent = "Error";
    }
}

document.getElementById("logout").addEventListener("click", (e) => {
    e.preventDefault();
    // Borrar la cookie del token
    document.cookie = "token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    // Redirigir al login
    window.location.href = "login.html";
});

init();