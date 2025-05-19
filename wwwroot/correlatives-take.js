import { verifyToken } from "./verifyToken.js";

async function cargarCorrelativas() {
    const token = document.cookie.replace(/(?:(?:^|.*;\s*)token\s*=\s*([^;]*).*$)|^.*$/, "$1");
    
    if (!token) {
        alert("Token not found, please re login.");
        window.location.href = "login.html";
    } else if (!(await verifyToken())) {
        alert("Token expired, please re login.");
        window.location.href = "login.html";
    }

    try {
        try {
            const response = await fetch("http://localhost:5000/api/v0/classes/correlatives?forW=Rendir", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
            });
    
            if (response.ok) {
                const result = await response.json();
                const correlativas = result.correlatives.map(correlativa => ({
                    year: correlativa.year,
                    name: correlativa.name,
                    condition: correlativa.condition,
                    plan: correlativa.plan
                }));
                correlativas.forEach(correlativa => {
                    let conditions = "";
                    for (const condition of correlativa.condition) {
                        conditions += condition + "<br>";
                    }
                    const fila = document.createElement('tr');
                    fila.innerHTML = `
                        <td>${correlativa.year}</td>
                        <td>${correlativa.name}</td>
                        <td>${conditions}</td>
                        <td>${correlativa.plan}</td>
                    `;
                    document.getElementById("state-table").appendChild(fila);
                    }
                );
            } else {
                const errorData = await response.text();
                alert(`Error: ${errorData}`);
            }
        } catch (error) {
            alert("Error during state fetch.");
        }
    } catch (error) {
        console.error('Error al cargar materias:', error);
    }
}

cargarCorrelativas();