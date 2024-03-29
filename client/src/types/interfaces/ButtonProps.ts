import { ReactNode } from "react";

export interface ButtonProps {
  children: ReactNode;
  onClick: () => void;
  color?: "orange" | "blue";
  className?: string;
  isDisabled?: boolean;
  type?: "button" | "submit" | "reset";
}
