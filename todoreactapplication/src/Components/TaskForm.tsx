import React from 'react';
import Task from '../Interfaces/ITask';
import TaskStatus from '../Enums/TaskStatus';


interface TaskFormProps {
  task: Task | null;
  handleInputChange: (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => void;
  saveTask: () => void;
}

const TaskForm: React.FC<TaskFormProps> = ({task, handleInputChange, saveTask }) => {
  return (
    <tr>
        <td>
            <input
                type="text"
                className="form-control"
                name="name"
                value={task?.name}
                onChange={handleInputChange}
            />
        </td>
        <td>
            <input
                type="number"
                className="form-control"
                name="priority"
                value={task?.priority}
                onChange={handleInputChange}
            />
        </td>
        <td>
            <select
            className="form-control"
            name="status"
            value={task?.status}
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
            <button className="btn btn-success" onClick={saveTask}>
            Save
            </button>
        </td>
    </tr>
  );
};

export default TaskForm;
