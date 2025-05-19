import { verifyToken } from "./verifyToken.js";

async function cargarCursada() {
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
            const response = await fetch("http://localhost:5000/api/v0/classes/courses", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
            });
            
            if (response.ok) {
                const result = await response.json();
                const cursos = result.courses.map(curso => ({
                    year: curso.year,
                    name: curso.name,
                    classId: curso.classId,
                    room: curso.room,
                    professor: curso.professor,
                    days: curso.days
                }));
                cursos.forEach(curso => {
                        const fila = document.createElement('tr');
                        fila.innerHTML = `
                            <td>${curso.year}</td>
                            <td>${curso.name}</td>
                            <td>${curso.classId}</td>
                            <td>${curso.room} Sede Pane</td>
                            <td>${curso.professor}</td>
                            <td>${curso.days}</td>
                        `;
                        document.getElementById("course-table").appendChild(fila);
                    });
            } else {
                const errorData = await response.text();
                alert(`Error: ${errorData}`);
            }
        } catch (error) {
            alert("Error during state fetch.");
        }
    } catch (error) {
        console.error('Error al cargar cursada:', error);
    }
}

cargarCursada();