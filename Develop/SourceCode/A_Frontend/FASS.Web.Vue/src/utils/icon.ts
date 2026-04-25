export const handleIcon = (icon = null) => {
  if (icon === null) {
    return icon;
  }
  if (["ep:", "ri:", "fa-solid:"].some(x => icon.startsWith(x))) {
    return icon;
  }
  if (icon.includes("fa-solid")) {
    return icon.replace(/.*fa-solid\s*fa-/g, "fa-solid:");
  }
  return null;
};
