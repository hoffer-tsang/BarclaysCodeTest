import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import TaskManager from '../Components/Task';

test('renders TaskManager component', () => {
  render(<TaskManager />);
  const taskManagerElement = screen.getByText(/Task Manager/i);
  expect(taskManagerElement).toBeInTheDocument();
});

// Add more tests as needed

test('adds a new task', () => {
    render(<TaskManager />);
    const addTaskButton = screen.getByText(/Add Task/i);
    const taskInput = screen.getByPlaceholderText('Add a new task');
  
    fireEvent.change(taskInput, { target: { value: 'New Task' } });
    fireEvent.click(addTaskButton);
  
    const newTask = screen.getByText(/New Task/i);
    expect(newTask).toBeInTheDocument();
  });

  