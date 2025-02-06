import axios from 'axios';
axios.defaults.baseURL = "http://localhost:5138";

const instance = axios.create();
instance.interceptors.response.use(
    response => response,
    error => {
        const dialog = document.createElement('dialog');
        dialog.textContent = error.message || 'Something went wrong!';
        document.body.appendChild(dialog);

        setTimeout(() => {
            dialog.close();
            dialog.remove();
        }, 5000);

        dialog.addEventListener('click', () => {
            dialog.close();
            dialog.remove();
        });

        dialog.showModal();

        return Promise.reject(error);
    }
);


export default {
  getTasks: async () => {
    try {
        const response = await axios.get('/items', {
        });
        return response.data;
    } catch (error) {
        throw error;
    }
  },
  addTask: async (name) => {
    axios.post('/items', {
       id: 0, name: name, isComplete: false
    }).then(result => {
      return getTasks();
    }).then(e =>{
      console.log(e.message);
    })
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete })
    //TODO
    return {};
  },

  deleteTask: async () => {
    console.log('deleteTask')
  }
};