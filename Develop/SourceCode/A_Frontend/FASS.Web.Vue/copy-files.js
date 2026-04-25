import fs from "fs-extra";
import path from "path";

async function copyFiles() {
  const srcDir = path.resolve("src/utils/map/dist/img");
  const destDir = path.resolve("dist/static/img");

  try {
    await fs.copy(srcDir, destDir);
    console.log("Files copied successfully!");
  } catch (err) {
    console.error("Error copying files:", err);
  }
}

copyFiles();
