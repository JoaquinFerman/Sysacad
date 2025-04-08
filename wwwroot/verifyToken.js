export const verifyToken = async () => {
    try {
        const token = document.cookie.replace(/(?:(?:^|.*;\s*)token\s*=\s*([^;]*).*$)|^.*$/, "$1");
        if(token) {
            const response = await fetch("http://localhost:5000/api/v0/token", {
                method: "GET",
                headers:{
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                }
            });
            
            if (response.ok) {
                return true;
            } else {
                return false;
            }
        }
    } catch {
        return false;
    }
}