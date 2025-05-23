import { verifyToken } from "./verifyToken.js";

document.getElementById('loginForm').addEventListener('submit', async function(event) {
    event.preventDefault();

    const id = document.getElementById('id').value;
    const password = document.getElementById('password').value;

    const loginData = { id, password };

    try {
        const response = await fetch("http://localhost:5000/api/v0/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(loginData)
        });

        if (response.ok) {
            const result = await response.json();
            if (result.token) {
                document.cookie = `token=${result.token}; path=/;`;
                window.location.href = "main.html"; 
            }
        } else {
            const errorData = await response.text();
            alert(`Error: ${errorData}`);
        }
    } catch (error) {
        alert("Error during login.");
    }
});

const init = async () => {
    if (await verifyToken()) {
        window.location.href = "main.html";
    }
}

init();