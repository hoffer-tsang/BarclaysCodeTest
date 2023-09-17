import { render, fireEvent, waitFor} from '@testing-library/react';
import axios from 'axios';
import TaskManager from '../Components/TaskManager';
import TaskStatus from '../Enums/TaskStatus';
import '@testing-library/jest-dom/extend-expect'

jest.mock('axios');

const mockedTasks = [
  {
    id: 1,
    name: 'Task 1',
    priority: 2,
    status: TaskStatus.NOT_STARTED,
  },
  {
    id: 2,
    name: 'Task 2',
    priority: 1,
    status: TaskStatus.COMPLETED,
  },
];

describe('TaskManager', () => {

  //Check rendering normally
  it('renders without errors', () => {
    render(<TaskManager />);
  });

  //Check get avaliable tasks
  it('get tasks on component mount', async () => {
    (axios.get as jest.Mock).mockResolvedValue({ data: { tasks: mockedTasks } });
    const { getByText } = render(<TaskManager />);

    await waitFor(() => {
      expect(getByText('Task Manager')).toBeInTheDocument();
      expect(getByText('Add New Task')).toBeInTheDocument();
      expect(getByText('Task 1')).toBeInTheDocument();
      expect(getByText('Task 2')).toBeInTheDocument();
    });
  });

  //Check adding new task
  it('adds a new task', async () => {

    const updatedTasks = [
      {
        id: 1,
        name: 'Task 1',
        priority: 2,
        status: TaskStatus.NOT_STARTED,
      },
      {
        id: 2,
        name: 'Task 2',
        priority: 1,
        status: TaskStatus.IN_PROGRESS,
      },
      {
        id: 3,
        name: 'New Task',
        priority: 3,
        status: TaskStatus.COMPLETED,
      },
    ];

    (axios.post as jest.Mock).mockResolvedValue({ data: {} });
    (axios.get as jest.Mock).mockResolvedValue({ data: { tasks: updatedTasks } });

    const { getByText, getByTestId } = render(<TaskManager />);
    
    fireEvent.click(getByText('Add New Task'));
    fireEvent.change(getByTestId('newName'), { target: { value: 'New Task' } });
    fireEvent.change(getByTestId('newPriority'), { target: { value: '3' } });
    fireEvent.change(getByTestId('newStatus'), { target: { value: 'COMPLETED' } });
    fireEvent.click(getByText('Save'));

    await waitFor(() => {
      expect(getByText('New Task')).toBeInTheDocument();
    });
  });

  //Edit Task
  it('edits a task', async () => {

    const updatedTasks = [
      {
        id: 1,
        name: 'Updated Task 1',
        priority: 4,
        status: TaskStatus.NOT_STARTED,
      },
      {
        id: 2,
        name: 'Task 2',
        priority: 1,
        status: TaskStatus.COMPLETED,
      }
    ];
    
    (axios.get as jest.Mock).mockResolvedValue({ data: { tasks: mockedTasks } });

    const { getByText, getByTestId} = render(<TaskManager />);
    await waitFor(() => {
      expect(getByText('Task 1')).toBeInTheDocument();
      expect(getByText('Task 2')).toBeInTheDocument();
    });

    (axios.put as jest.Mock).mockResolvedValue({ data: {} });
    (axios.get as jest.Mock).mockResolvedValue({ data: { tasks: updatedTasks } });

    fireEvent.click(getByTestId('editTask1'));
    fireEvent.change(getByTestId('editName1'), { target: { value: 'Updated Task 1' } });
    fireEvent.change(getByTestId('editPriority1'), { target: { value: '4' } });
    fireEvent.change(getByTestId('editStatus1'), { target: { value: TaskStatus.NOT_STARTED } });
    fireEvent.click(getByTestId('save1'));

    await waitFor(() => {
      expect(axios.put).toHaveBeenCalledWith('https://localhost:7271/api/v1/Task/1', {
        id: 1,
        name: 'Updated Task 1',
        priority: 4,
        status: 0,
      });
      expect(getByText('Updated Task 1')).toBeInTheDocument();
    });
  });

  //Delete Task
  it('deletes a task', async () => {
    const initialTask = [
      {
        id: 1,
        name: 'Task 1',
        priority: 2,
        status: 0,
      },
      {
        id: 2,
        name: 'Task 2',
        priority: 1,
        status: 2,
      },
    ];

    const deletedTasks = [
      {
        id: 1,
        name: 'Task 1',
        priority: 2,
        status: 0,
      }
    ];

    (axios.get as jest.Mock).mockResolvedValue({ data: { tasks: initialTask } });

  const { getByText, getByTestId, queryByText } = render(<TaskManager />);
  
  await waitFor(() => {
    expect(getByText('Task 1')).toBeInTheDocument();
    expect(getByText('Task 2')).toBeInTheDocument();
  });

  (axios.delete as jest.Mock).mockResolvedValue({ data: {} });
  (axios.get as jest.Mock).mockResolvedValue({ data: { tasks: deletedTasks } });

  fireEvent.click(getByTestId('deleteTask2')); 
  await waitFor(() => {
    expect(axios.delete).toHaveBeenCalledWith('https://localhost:7271/api/v1/Task/2');
    expect(queryByText('Task 2')).not.toBeInTheDocument();
    });
  });

  //Sorting task by name
  it('sorts tasks by name', async () => {
    const { getByText, getAllByRole } = render(<TaskManager />);

    fireEvent.click(getByText('Name'));
    const rows = getAllByRole('row');
    const taskNames = rows.slice(1).map(row => {
      const cells = row.getElementsByTagName('td');
      return cells[0].textContent; 
    });

    const sortedTaskNames = [...taskNames].sort();
    expect(taskNames).toEqual(sortedTaskNames);
  });

  //Sort task by priority
  it('sorts tasks by priority', async () => {
    const { getByText, getAllByRole } = render(<TaskManager />);

    fireEvent.click(getByText('Priority'));
    const rows = getAllByRole('row');
    const taskPriority = rows.slice(1).map(row => {
      const cells = row.getElementsByTagName('td');
      return cells[1].textContent; 
    });

    const sortedTaskPriority = [...taskPriority].sort();
    expect(taskPriority).toEqual(sortedTaskPriority);
  });

  //Sort task by status
  it('sorts tasks by status', async () => {
    const { getByText, getAllByRole } = render(<TaskManager />);
    
    fireEvent.click(getByText('Status'));
    const rows = getAllByRole('row');
    const taskStatus = rows.slice(2).map(row => {
      const cells = row.getElementsByTagName('td');
      return cells[1].textContent; 
    });

    const sortedTaskStatus = [...taskStatus].sort();
    expect(taskStatus).toEqual(sortedTaskStatus);
  });
});
