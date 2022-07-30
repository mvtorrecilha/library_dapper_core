import axios from 'axios'
import Axios from 'axios'



export default {
    async listBooks(){
        var user = JSON.parse(localStorage.getItem('loggedUser'));        
        return axios.get('api/books',
            { headers: {'Authorization': `Bearer ${user.idToken}`}
        });       
    },
    async borrowBook(id) {
        var user = JSON.parse(localStorage.getItem('loggedUser'));
        return axios.post(`/api/books/${id}/student/${user.email}`, null,
            { headers: {'Authorization': `Bearer ${user.idToken}`}
        });  
    }
}