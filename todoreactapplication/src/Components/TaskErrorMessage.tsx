// components/TaskErrorMessage.tsx

import React from 'react';

interface TaskErrorMessageProps {
  errorMessage: string | null;
}

const TaskErrorMessage: React.FC<TaskErrorMessageProps> = ({ errorMessage }) => {
  return (
    <div>
      {errorMessage && (
        <div className="alert alert-danger" role="alert">
          {errorMessage}
        </div>
      )}
    </div>
  );
};

export default TaskErrorMessage;
