// import axios from 'axios';

// export default {
//   getTasks: async () => {
//     try {
//         const response = await axios.get('/items', {
//         });
//         return response.data;
//     } catch (error) {
//         throw error;
//     }
//   },
//   addTask: async (name) => {
//     axios.post('/items', {
//        id: 0, name: name, isComplete: false
//     }).then(result => {
//       return getTasks();
//     }).then(e =>{
//       console.log(e.message);
//     })
//   },

//   setCompleted: async (id, isComplete) => {
//     console.log('setCompleted', { id, isComplete })
//     //TODO
//     return {};
//   },

//   deleteTask: async () => {
//     console.log('deleteTask')
//   }
// };

import axios from 'axios';
import myAxios from './axiosConfiguration';

export default {
  getTasks: async () => {
    const result = await myAxios.get(`/tasks`)
    if (result == undefined || result.data == undefined)
      return [];
    else
      return result.data;
  },

  addTask: async (name) => {
    const result = await myAxios.post(`/tasks`, { Id: 0, Name: name, IsCompelte: false })
    return {};
  },

  setCompleted: async (id, isComplete) => {
    const result = await myAxios.put(`/tasks/${id}`, { Id: id, Name: "", IsCompelte: isComplete });
    return {};
  },

  deleteTask: async (id) => {
    const result = myAxios.delete(`/tasks/${id}`);
    return {};
  }
};