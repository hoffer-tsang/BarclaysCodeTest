import TaskStatus from "../Enums/TaskStatus";

interface Task {
    id: number;
    name: string;
    priority: number;
    status: TaskStatus;
  }

export default Task