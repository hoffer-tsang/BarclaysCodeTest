import axios from 'axios';

export default axios.create({
    baseURL: 'https://localhost:7271/api/v1/Task'
})