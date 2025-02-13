
import React, { useEffect, useState } from 'react';
import service from './service.js';

function App() {
  const [newTodo, setNewTodo] = useState("");
  const [todos, setTodos] = useState([]);

  async function getTodos() {
    const todos = await service.getTasks();
    setTodos(todos);
  }

  async function createTodo(e) {
    e.preventDefault();
    await service.addTask(newTodo);
    setNewTodo(""); // Clear input
    await getTodos(); // Refresh tasks list
  }

  async function updateCompleted(todo, isComplete) {
    await service.setCompleted(todo.id, isComplete);
    await getTodos(); // Refresh tasks list
  }

  async function deleteTodo(id) {
    await service.deleteTask(id);
    await getTodos(); // Refresh tasks list
  }

  useEffect(() => {
    getTodos();
  }, []);

  return (
    <section className="todo-container">
      <header className="header-section">
        <h1 className="header-title">todos</h1>
        <form onSubmit={createTodo}>
          <input 
            className="input-field" 
            placeholder="Well, let's take on the day" 
            value={newTodo} 
            onChange={(e) => setNewTodo(e.target.value)} 
          />
        </form>
      </header>
      <section className="main-section">
        <ul className="todo-list">
          {todos.map(todo => (
            <li className={`todo-item ${todo.isComplete ? "completed" : ""}`} key={todo.id}>
              <div className="todo-view">
                <input 
                  className="checkbox" 
                  type="checkbox" 
                  checked={todo.isComplete} 
                  onChange={(e) => updateCompleted(todo, e.target.checked)} 
                />
                <label>{todo.name}</label>
                <button className="delete-button" onClick={() => deleteTodo(todo.id)}>×</button>
              </div>
            </li>
          ))}
        </ul>
      </section>
    </section>
  );
}

export default App;
