
import React, { useState, useEffect } from 'react';
//import axios from 'axios';
import TaskList from './TaskList';
import TaskForm from './TaskForm';
import TaskErrorMessage from './TaskErrorMessage';
import Task from '../Interfaces/ITask';
import PostTask from '../Interfaces/IPostTask';
import PutTask from '../Interfaces/IPutTask';
import TaskStatus from '../Enums/TaskStatus';

export const mappedStatusToNumber = (status: TaskStatus | null): number => {
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
      statusValue = NaN; 
  }
  return statusValue;
}

const axios = require('axios');

const TaskManager: React.FC = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [newTask, setNewTask] = useState<Task | null>(null);
  const [editNewTask, setEditNewTask] = useState<Task | null>(null);
  const [sortColumn, setSortColumn] = useState<string>('name');
  const [sortDirection, setSortDirection] = useState<number>(1);
  const [errorMessage, setErrorMessage] = useState<string | null>(null);
  const baseUrL = 'https://localhost:7271/api/v1/Task';

  const fetchTasks = async () => {
    try {
      const response = await axios.get(`${baseUrL}`);
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
      await axios.put(`${baseUrL}/${editNewTask.id}`, putTask);
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
      await axios.post(`${baseUrL}`, postTask);
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
      await axios.delete(`${baseUrL}/${task.id}`);
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
      <TaskErrorMessage errorMessage={errorMessage} />
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
        <TaskList tasks={sortedTasks} editTask={editTask} saveEditedTask={saveEditedTask} deleteTask={deleteTask} handleInputChange={handleEditChange} editNewTask={editNewTask}/>
        {newTask && (
          <TaskForm task={newTask} handleInputChange={handleInputChange} saveTask={saveNewTask}/>
        )}
      </table>
    </div>
  );
};

export default TaskManager;
