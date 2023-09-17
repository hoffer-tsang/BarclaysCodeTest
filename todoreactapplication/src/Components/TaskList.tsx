import React from 'react';
import Task from '../Interfaces/ITask';
import TaskStatus from '../Enums/TaskStatus';

interface TaskListProps {
    tasks: Task[];
    editTask: (task: Task) => void;
    saveEditedTask: () => void;
    deleteTask: (task: Task) => void;
    handleInputChange: (event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => void;
    editNewTask: Task | null;
}

const TaskList: React.FC<TaskListProps> = ({tasks, editTask, saveEditedTask, deleteTask, handleInputChange, editNewTask }) => {
  return (
    <tbody>
      {tasks.map((task) => (
            <tr key={task.id}>
              <td>{editNewTask?.id === task.id ? (
                <input
                  data-testid={'editName' + task.id}
                  type="text"
                  className="form-control"
                  name="name"
                  value={editNewTask.name}
                  onChange={handleInputChange}
                />
              ) : (
                task.name
              )}</td>
              <td>{editNewTask?.id === task.id ? (
                <input
                  data-testid={'editPriority' + task.id}
                  type="number"
                  className="form-control"
                  name="priority"
                  value={editNewTask.priority}
                  onChange={handleInputChange}
                />
              ) : (
                task.priority
              )}</td>
              <td>{editNewTask?.id === task.id ? (
                <select
                  data-testid={'editStatus' + task.id}
                  className="form-control"
                  name="status"
                  value={editNewTask.status}
                  onChange={handleInputChange}
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
                  <button data-testid={'save' + task.id} className="btn btn-success" onClick={saveEditedTask}>
                    Save
                  </button>
                ) : (
                  <>
                    <button data-testid={'editTask' + task.id} className="btn btn-primary" onClick={() => editTask(task)}>
                      Edit
                    </button>
                    <button data-testid={'deleteTask' + task.id} className="btn btn-danger ml-2" onClick={() => deleteTask(task)}>
                      Delete
                    </button>
                  </>
                )}
              </td>
            </tr>
          ))}
    </tbody>
  );
};

export default TaskList;
