// src/Task.tsx

import React, { useState, useEffect } from 'react';
import axios from '../API/axios';
enum TaskStatus {
  NOT_STARTED = 'Not Started',
  IN_PROGRESS = 'In Progress',
  COMPLETED = 'Completed',
}

interface Task {
  id: number;
  name: string;
  priority: number;
  status: TaskStatus;
}

interface PostTask {
  name: string;
  priority: number;
  status: number;
}

interface PutTask {
    id: number;
    name: string;
    priority: number;
    status: number;
}

function mappedStatusToNumber(status: TaskStatus): number {
    let statusValue: number;
  
    switch (status) {
      case TaskStatus.NOT_STARTED:
        statusValue = 0;
        break;
      case TaskStatus.IN_PROGRESS:
        statusValue = 1;
        break;
      case TaskStatus.COMPLETED:
        statusValue = 2;
        break;
      default:
        // Handle any other status values here if needed.
        statusValue = NaN; // Default to 0 for unknown status.
    }

    return statusValue;
}  

const TaskManager: React.FC = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [newTask, setNewTask] = useState<Task | null>(null);
  const [editNewTask, setEditNewTask] = useState<Task | null>(null);
  const [sortColumn, setSortColumn] = useState<string>('name');
  const [sortDirection, setSortDirection] = useState<number>(1);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const fetchTasks = async () => {
    try {
      const response = await axios.get("");
      setErrorMessage(null);
      const tasksWithMappedStatus = response.data.tasks.map((task: any) => ({
        ...task,
        status:
          task.status === 0
            ? TaskStatus.NOT_STARTED
            : task.status === 1
            ? TaskStatus.IN_PROGRESS
            : task.status === 2
            ? TaskStatus.COMPLETED
            : null,
      }));
      setTasks(tasksWithMappedStatus);
    } catch (error) {
      setErrorMessage('Failed to fetch tasks.');
      console.error('Error fetching tasks:', error);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, []);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
    if (newTask) {
      setNewTask({
        ...newTask,
        [name]: name === 'priority' ? parseInt(value, 10) : value,
      });
    }
  };

  const handleEditChange = (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = event.target;
    if (editNewTask) {
      setEditNewTask({
        ...editNewTask,
        [name]: name === 'priority' ? parseInt(value, 10) : value,
      });
    }
  };

  const editTask = (task: Task) => {
    setEditNewTask({ ...task });
  };
  
  const saveEditedTask = async () => {
    if (!editNewTask) {
      return;
    }

    if (editNewTask.name.trim() === '') {
        setErrorMessage('Task name cannot be empty.');
        return;
      }
    if (tasks.some((t) => t.name === editNewTask.name && t.id !== editNewTask.id)) {
      setErrorMessage('Task name must be unique.');
      return;
    }

    setErrorMessage(null);
    try {
        const putTask: PutTask = {
            id: editNewTask.id,
            name: editNewTask.name,
            priority: editNewTask.priority,
            status: mappedStatusToNumber(editNewTask.status)
        };
      await axios.put(`${editNewTask.id}`, putTask);
      fetchTasks();
      setEditNewTask(null);
    } catch (error) {
      console.error('Error editing task:', error);
      setErrorMessage('Error saving task');
    }
  };

  const saveNewTask = async () => {
    if (!newTask) {
      return;
    }

    if (newTask.name.trim() === '') {
        setErrorMessage('Task name cannot be empty.');
        return;
      }
    if (tasks.some((task) => task.name === newTask.name)) {
      setErrorMessage('Task name must be unique.');
      return;
    }

    setErrorMessage(null);
    try {
        const postTask: PostTask = {
            name: newTask.name,
            priority: newTask.priority,
            status: mappedStatusToNumber(newTask.status)
        };
      await axios.post("", postTask);
      fetchTasks();
      setNewTask(null);
    } catch (error) {
      console.error('Error editing task:', error);
      setErrorMessage('Error saving task');
    }
  };

  const deleteTask = async (task: Task) => {
    if (task.status !== TaskStatus.COMPLETED)
    {
      setErrorMessage("Task status must be completed before delete");
      return;
    }
    try {
      await axios.delete(`${task.id}`);
      fetchTasks();
    } catch (error) {
      console.error('Error deleting task:', error);
      setErrorMessage("Error deleting task:");
    }
  };

  const handleSort = (columnName: string) => {
    if (columnName === sortColumn) {
      setSortDirection(-sortDirection);
    } else {
      setSortColumn(columnName);
      setSortDirection(1);
    }
  };

  const sortedTasks = [...tasks].sort((a, b) => {
    if (sortColumn === 'name') {
      return a.name.localeCompare(b.name) * sortDirection;
    } else if (sortColumn === 'priority') {
      return (a.priority - b.priority) * sortDirection;
    } else if (sortColumn === 'status') {
      return a.status.localeCompare(b.status) * sortDirection;
    }
    return 0;
  });

  const addNewTask = () => {
    setNewTask({
      id: 0,
      name: '',
      priority: 1,
      status: TaskStatus.NOT_STARTED,
    });
  };

  return (
    <div className="container mt-5">
      <h1 className="text-center mb-4">Task Manager</h1>
      {errorMessage && (
          <div className="alert alert-danger" role="alert">
            {errorMessage}
          </div>
      )}
      <button className="btn btn-success mb-2 float-right" onClick={addNewTask}>
        Add New Task
      </button>
      <table className="table table-bordered">
        <thead>
          <tr>
            <th onClick={() => handleSort('name')}>Name</th>
            <th onClick={() => handleSort('priority')}>Priority</th>
            <th onClick={() => handleSort('status')}>Status</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {sortedTasks.map((task) => (
            <tr key={task.id}>
              <td>{editNewTask?.id === task.id ? (
                <input
                  type="text"
                  className="form-control"
                  name="name"
                  value={editNewTask.name}
                  onChange={handleEditChange}
                />
              ) : (
                task.name
              )}</td>
              <td>{editNewTask?.id === task.id ? (
                <input
                  type="number"
                  className="form-control"
                  name="priority"
                  value={editNewTask.priority}
                  onChange={handleEditChange}
                />
              ) : (
                task.priority
              )}</td>
              <td>{editNewTask?.id === task.id ? (
                <select
                  className="form-control"
                  name="status"
                  value={editNewTask.status}
                  onChange={handleEditChange}
                >
                  {Object.values(TaskStatus).map((status) => (
                    <option key={status} value={status}>
                      {status}
                    </option>
                  ))}
                </select>
              ) : (
                task.status
              )}</td>
              <td>
                {editNewTask?.id === task.id ? (
                  <button className="btn btn-success" onClick={saveEditedTask}>
                    Save
                  </button>
                ) : (
                  <>
                    <button className="btn btn-primary" onClick={() => editTask(task)}>
                      Edit
                    </button>
                    <button className="btn btn-danger ml-2" onClick={() => deleteTask(task)}>
                      Delete
                    </button>
                  </>
                )}
              </td>
            </tr>
          ))}
          {newTask && (
            <tr>
              <td>
                <input
                  type="text"
                  className="form-control"
                  name="name"
                  value={newTask.name}
                  onChange={handleInputChange}
                />
              </td>
              <td>
                <input
                  type="number"
                  className="form-control"
                  name="priority"
                  value={newTask.priority}
                  onChange={handleInputChange}
                />
              </td>
              <td>
                <select
                  className="form-control"
                  name="status"
                  value={newTask.status}
                  onChange={handleInputChange}
                >
                  {Object.values(TaskStatus).map((status) => (
                    <option key={status} value={status}>
                      {status}
                    </option>
                  ))}
                </select>
              </td>
              <td>
                <button className="btn btn-success" onClick={saveNewTask}>
                  Save
                </button>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default TaskManager;
